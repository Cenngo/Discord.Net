using Discord.API;
using Discord.API.Rest;
using Discord.Logging;
using Discord.SlashCommands.Builders;
using Discord.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Discord.SlashCommands
{
    public class SlashCommandService : IDisposable
    {
        public event Func<LogMessage, Task> Log { add { _logEvent.Add(value); } remove { _logEvent.Remove(value); } }
        internal readonly AsyncEvent<Func<LogMessage, Task>> _logEvent = new AsyncEvent<Func<LogMessage, Task>>();

        public event Func<Optional<SlashCommandInfo>, ISlashCommandContext, IResult, Task> CommandExecuted { add { _commandExecutedEvent.Add(value); } remove { _commandExecutedEvent.Remove(value); } }
        internal readonly AsyncEvent<Func<Optional<SlashCommandInfo>, ISlashCommandContext, IResult, Task>> _commandExecutedEvent = new AsyncEvent<Func<Optional<SlashCommandInfo>, ISlashCommandContext, IResult, Task>>();


        private readonly ConcurrentDictionary<Type, SlashModuleInfo> _typedModuleDefs;
        private readonly SlashCommandMap _commandMap;
        private readonly HashSet<SlashModuleInfo> _moduleDefs;
        private readonly SemaphoreSlim _lock;
        internal readonly Logger _cmdLogger;
        internal readonly LogManager _logManager;

        public IReadOnlyList<SlashModuleInfo> Modules => _moduleDefs.ToList();
        public IReadOnlyList<SlashCommandInfo> Commands => _moduleDefs.SelectMany(x => x.Commands).ToList();
        public IDiscordClient Client { get; }

        public SlashCommandService (IDiscordClient discord) : this(discord, new SlashCommandServiceConfig()) { }

        public SlashCommandService(IDiscordClient discord, SlashCommandServiceConfig config)
        {
            _lock = new SemaphoreSlim(1, 1);
            _typedModuleDefs = new ConcurrentDictionary<Type, SlashModuleInfo>();
            _moduleDefs = new HashSet<SlashModuleInfo>();

            _logManager = new LogManager(LogSeverity.Debug);
            _logManager.Message += async msg => await _logEvent.InvokeAsync(msg).ConfigureAwait(false);
            _cmdLogger = _logManager.CreateLogger("Command");

            _commandMap = new SlashCommandMap(this);

            Client = discord;
        }

        public async Task<IEnumerable<SlashModuleInfo>> AddModules(Assembly assembly, IServiceProvider services)
        {
            services = services ?? EmptyServiceProvider.Instance;

            await _lock.WaitAsync().ConfigureAwait(false);

            try
            {
                var types = await ModuleClassBuilder.SearchAsync(assembly, this);
                var moduleDefs = await ModuleClassBuilder.BuildAsync(types, this, services);

                foreach(var info in moduleDefs)
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

        public async Task SyncCommands ( )
        {
            DiscordRestApiClient restClient;
            //ulong applicationId = ( await Client.GetApplicationInfoAsync() ).Id;
            ulong applicationId = 666632416554909706;

            if (Client is DiscordSocketClient socketClient)
                restClient = socketClient.ApiClient;
            else if (Client is DiscordShardedClient shardedClient)
                restClient = shardedClient.ApiClient;
            else
                throw new InvalidOperationException("Operation is not supported for the provided Discord Client type.");

            var creationParams = new List<CreateApplicationCommandParams>();

            foreach(var module in Modules)
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

            foreach(var args in creationParams)
            {
                var json = JsonConvert.SerializeObject(args, Formatting.Indented, new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.All,
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                });
                await restClient.CreateGuildApplicationCommand(applicationId, 656609553437163551, args);
            }
        }

        private void LoadModuleInternal ( SlashModuleInfo module )
        {
            _moduleDefs.Add(module);

            foreach(var command in module.Commands)
            {
                _commandMap.AddCommand(command);
            }
        }

        public async Task<bool> RemoveModuleAsync(Type type)
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


            foreach(var command in moduleInfo.Commands)
            {
                _commandMap.RemoveCommand(command);
            }

            return true;
        }

        public async Task ExecuteAsync ( ISlashCommandContext context, string input, IServiceProvider services )
        {
            services = services ?? EmptyServiceProvider.Instance;

            var command = _commandMap.GetCommands(input).First();

            if(command == null)
            {
                await _cmdLogger.DebugAsync($"Unknown slash command, skipping execution ({input.ToUpper()})");
                return;
            }
            await command.ExecuteAsync(context, services).ConfigureAwait(false);
        }

        public void Dispose ( ) => throw new NotImplementedException();
    }
}
