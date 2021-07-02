using Discord.SlashCommands.Builders;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord.API;
using Discord.WebSocket;

namespace Discord.SlashCommands
{
    public class SlashCommandInfo
    {
        private readonly MethodInfo _methodInfo;
        private readonly Func<ISlashCommandContext, object[], IServiceProvider, SlashCommandInfo, Task> _action;

        public SlashCommandService CommandService { get; }
        public string Name { get; }
        public string Description { get; }
        public bool DefaultPermission { get; }
        public IReadOnlyList<SlashParameterInfo> Parameters { get; }
        public SlashModuleInfo Module { get; }
        public SlashGroupInfo Group { get; }
        public IReadOnlyList<Attribute> Attributes { get; }

        internal SlashCommandInfo (SlashCommandBuilder builder, SlashModuleInfo module, SlashCommandService commandService)
        {
            CommandService = commandService;
            Module = module;

            Name = builder.Name;
            Description = builder.Description;
            Group = builder.Group;
            DefaultPermission = builder.DefaultPermission;
            Parameters = builder.Parameters.Select(x => x.Build(this)).ToImmutableArray();
            Attributes = builder.Attributes.ToImmutableArray();

            _action = builder.Callback;
        }

        public async Task ExecuteAsync (ISlashCommandContext context, IServiceProvider services )
        {
            services = services ?? EmptyServiceProvider.Instance;

            if (context.Interaction is SocketCommandInteraction commandInteraction)
            {
                await ExecuteAsync(context, Parameters, commandInteraction.Data, services);
            }
        }

        public async Task<IResult> ExecuteAsync(ISlashCommandContext context, IEnumerable<SlashParameterInfo> paramList,
            IEnumerable<SocketInteractionParameter> argList, IServiceProvider services)
        {
            services = services ?? EmptyServiceProvider.Instance;

            try
            {
                object[] args = GenerateArgs(paramList, argList);

                _ = Task.Run(async ( ) =>
                {
                    await ExecuteInternalAsync(context, args, services).ConfigureAwait(false);
                });
                return new ExecuteResult(null, null, true);
            }
            catch (Exception ex)
            {
                return new ExecuteResult(SlashCommandError.Exception, ex.Message, false);
            }
        }

        private async Task<IResult> ExecuteInternalAsync ( ISlashCommandContext context, object[] args, IServiceProvider services )
        {
            await Module.CommandService._cmdLogger.DebugAsync($"Executing {GetLogString(context)}").ConfigureAwait(false);

            try
            {
                var task = _action(context, args, services, this);

                await task.ConfigureAwait(false);
                var result = new ExecuteResult(null, null, true);
                await Module.CommandService._commandExecutedEvent.InvokeAsync(this, context, result).ConfigureAwait(false);

                return result;
            }
            catch (Exception ex)
            {
                return new ExecuteResult(SlashCommandError.Exception, ex.Message, false);
            }
            finally
            {
                await Module.CommandService._cmdLogger.VerboseAsync($"Executed {GetLogString(context)}").ConfigureAwait(false);
            }
        }

        private object[] GenerateArgs (IEnumerable<SlashParameterInfo> paramList, IEnumerable<SocketInteractionParameter> argList )
        {
            var result = new List<object>();

            foreach(var parameter in paramList)
            {
                var arg = argList.First(x => x.Name == parameter.Name);

                if (arg == null && parameter.IsRequired)
                    throw new InvalidOperationException("Command was invoked with too few parameters");
                else
                {
                    if (arg.Value is Optional<object> optional)
                        result.Add(optional.Value);
                    else
                        result.Add(arg.Value);
                }
            }

            return result.ToArray();
        }

        private string GetLogString ( ISlashCommandContext context)
        {
            if (context.Guild != null)
                return $"\"{Name}\" for {context.User} in {context.Guild}/{context.Channel}";
            else
                return $"\"{Name}\" for {context.User} in {context.Channel}";
        }
    }
}
