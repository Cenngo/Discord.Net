using Discord.API;
using Discord.API.Rest;
using Discord.Logging;
using Discord.Rest;
using Discord.SlashCommands.Builders;
using Discord.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Discord.SlashCommands
{
    public class SlashCommandService : IDisposable
    {
        public event Func<LogMessage, Task> Log { add { _logEvent.Add(value); } remove { _logEvent.Remove(value); } }
        internal readonly AsyncEvent<Func<LogMessage, Task>> _logEvent = new AsyncEvent<Func<LogMessage, Task>>();

        public event Func<Optional<SlashCommandInfo>, ISlashCommandContext, IResult, Task> CommandExecuted { add { _commandExecutedEvent.Add(value); } remove { _commandExecutedEvent.Remove(value); } }
        internal readonly AsyncEvent<Func<Optional<SlashCommandInfo>, ISlashCommandContext, IResult, Task>> _commandExecutedEvent = new AsyncEvent<Func<Optional<SlashCommandInfo>, ISlashCommandContext, IResult, Task>>();

        public event Func<Optional<SlashInteractionInfo>, ISlashCommandContext, IResult, Task> InteractionExecuted { add { _interactionExecutedEvent.Add(value); } remove { _interactionExecutedEvent.Remove(value); } }
        internal readonly AsyncEvent<Func<Optional<SlashInteractionInfo>, ISlashCommandContext, IResult, Task>> _interactionExecutedEvent = new AsyncEvent<Func<Optional<SlashInteractionInfo>, ISlashCommandContext, IResult, Task>>();


        private readonly ConcurrentDictionary<Type, SlashModuleInfo> _typedModuleDefs;
        private readonly SlashCommandMap<SlashCommandInfo> _commandMap;
        private readonly SlashCommandMap<SlashInteractionInfo> _interactionCommandMap;
        private readonly HashSet<SlashModuleInfo> _moduleDefs;
        private readonly SemaphoreSlim _lock;
        private readonly ulong _applicationId;
        internal readonly Logger _cmdLogger;
        internal readonly LogManager _logManager;

        internal readonly bool _runAsync, _throwOnError;

        public IReadOnlyList<SlashModuleInfo> Modules => _moduleDefs.ToList();
        public IReadOnlyList<SlashCommandInfo> Commands => _moduleDefs.SelectMany(x => x.Commands).ToList();
        public IReadOnlyCollection<SlashInteractionInfo> Interacions => _moduleDefs.SelectMany(x => x.Interactions).ToList();
        public BaseSocketClient Client { get; }

        public SlashCommandService (BaseSocketClient discord) : this(discord, new SlashCommandServiceConfig()) { }

        public SlashCommandService (BaseSocketClient discord, SlashCommandServiceConfig config)
        {
            _lock = new SemaphoreSlim(1, 1);
            _typedModuleDefs = new ConcurrentDictionary<Type, SlashModuleInfo>();
            _moduleDefs = new HashSet<SlashModuleInfo>();

            _logManager = new LogManager(LogSeverity.Debug);
            _logManager.Message += async msg => await _logEvent.InvokeAsync(msg).ConfigureAwait(false);
            _cmdLogger = _logManager.CreateLogger("Command");

            _commandMap = new SlashCommandMap<SlashCommandInfo>(this);
            _interactionCommandMap = new SlashCommandMap<SlashInteractionInfo>(this);

            Client = discord;
            _applicationId = Client.GetApplicationInfoAsync().GetAwaiter().GetResult().Id;

            _runAsync = config.RunAsync;
            _throwOnError = config.ThrowOnError;
        }

        public async Task<IEnumerable<SlashModuleInfo>> AddModules (Assembly assembly, IServiceProvider services)
        {
            services = services ?? EmptyServiceProvider.Instance;

            await _lock.WaitAsync().ConfigureAwait(false);

            try
            {
                var types = await ModuleClassBuilder.SearchAsync(assembly, this);
                var moduleDefs = await ModuleClassBuilder.BuildAsync(types, this, services);

                foreach (var info in moduleDefs)
                {
                    _typedModuleDefs[info.Key] = info.Value;
                    LoadModuleInternal(info.Value);
                }
                return moduleDefs.Values;
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task SyncCommands (IGuild guild = null)
        {
            DiscordRestApiClient restClient;

            if (Client is DiscordSocketClient socketClient)
                restClient = socketClient.ApiClient;
            else if (Client is DiscordShardedClient shardedClient)
                restClient = shardedClient.ApiClient;
            else
                throw new InvalidOperationException("Operation is not supported for the provided Discord Client type.");

            var creationParams = new List<CreateApplicationCommandParams>();

            foreach (var module in Modules)
            {
                if (string.IsNullOrEmpty(module.Name))
                {
                    var args = module.Commands.AsEnumerable().GroupParseApplicationCommandParams();
                    creationParams.AddRange(args);
                }
                else
                {
                    if (module.TryParseApplicationCommandParams(out var args))
                        creationParams.Add(args);
                }
            }

            var existing = await Rest.SlashCommandHelper.GetApplicationCommands(Client, _applicationId, guild, null);

            var missing = existing.Where(x => creationParams.Any(y => y.Name == x.Name));

            foreach (var command in missing)
            {
                await Rest.SlashCommandHelper.DeleteApplicationCommand(Client, _applicationId, command.Id, guild, null).ConfigureAwait(false);
                existing.ToList().Remove(command);
            }

            foreach (var args in creationParams)
            {
                ApplicationCommand result;

                if (guild != null)
                    result = await restClient.CreateGuildApplicationCommand(_applicationId, guild.Id, args).ConfigureAwait(false);
                else
                    result = await restClient.CreateGlobalApplicationCommand(_applicationId, args).ConfigureAwait(false);

                if (result == null)
                    await _cmdLogger.WarningAsync($"Command could not be registered ({args.Name})").ConfigureAwait(false);
            }
        }

        public async Task AddCommandsToGuild ( IGuild guild, params SlashCommandInfo[] commands )
        {
            if (guild == null)
                throw new ArgumentException($"{nameof(guild)} cannot be null to call this function.");

            foreach (var com in commands)
            {
                ApplicationCommand result = await Client.ApiClient.CreateGuildApplicationCommand(_applicationId, guild.Id,
                    com.ParseApplicationCommandParams(), null);

                if (result == null)
                    await _cmdLogger.WarningAsync($"Command could not be registered ({com.Name})").ConfigureAwait(false);
            }
        }

        public async Task AddModulesToGuild ( IGuild guild, params SlashModuleInfo[] modules )
        {
            if (guild == null)
                throw new ArgumentException($"{nameof(guild)} cannot be null to call this function.");

            foreach (var module in Modules)
            {
                if(module.TryParseApplicationCommandParams(out var args))
                {
                    ApplicationCommand result = await Client.ApiClient.CreateGuildApplicationCommand(_applicationId, guild.Id, args, null);

                    if (result == null)
                        await _cmdLogger.WarningAsync($"Module could not be registered ({module.Name})").ConfigureAwait(false);
                }
            }
        }

        private void LoadModuleInternal (SlashModuleInfo module)
        {
            _moduleDefs.Add(module);

            foreach (var command in module.Commands)
                _commandMap.AddCommand(command);

            foreach (var internalCommand in module.Interactions)
                _interactionCommandMap.AddCommand(internalCommand);
        }

        public async Task<bool> RemoveModuleAsync (Type type)
        {
            await _lock.WaitAsync().ConfigureAwait(false);

            try
            {
                if (!_typedModuleDefs.TryRemove(type, out var module))
                    return false;

                return RemoveModuleInternal(module);
            }
            finally
            {
                _lock.Release();
            }
        }

        private bool RemoveModuleInternal (SlashModuleInfo moduleInfo)
        {
            if (!_moduleDefs.Remove(moduleInfo))
                return false;


            foreach (var command in moduleInfo.Commands)
            {
                _commandMap.RemoveCommand(command);
            }

            return true;
        }

        public async Task ExecuteCommandAsync (ISlashCommandContext context, string input, IServiceProvider services)
        {
            services = services ?? EmptyServiceProvider.Instance;

            var command = _commandMap.GetCommands(input).First();

            if (command == null)
            {
                await _cmdLogger.DebugAsync($"Unknown slash command, skipping execution ({input.ToUpper()})");
                return;
            }
            await command.ExecuteAsync(context, services).ConfigureAwait(false);
        }

        public async Task ExecuteInteractionAsync (ISlashCommandContext context, string input, IServiceProvider services)
        {
            services = services ?? EmptyServiceProvider.Instance;

            var command = _interactionCommandMap.GetCommands(input).First();

            if (command == null)
            {
                await _cmdLogger.DebugAsync($"Unknown custom interaction id, skipping execution ({input.ToUpper()})");
                return;
            }
            await command.ExecuteAsync(context, services).ConfigureAwait(false);
        }

        public void Dispose ( ) => throw new NotImplementedException();
    }
}
