using Discord.SlashCommands.Builders;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord.API;
using Discord.WebSocket;
using System.Runtime.ExceptionServices;

namespace Discord.SlashCommands
{
    public class SlashCommandInfo : IExecutableInfo
    {
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

        public async Task<IResult> ExecuteAsync (ISlashCommandContext context, IServiceProvider services )
        {
            if (context.Interaction is SocketCommandInteraction commandInteraction)
                return await ExecuteAsync(context, Parameters, commandInteraction.Data, services);
            else
                return ExecuteResult.FromError(SlashCommandError.ParseFailed, $"Provided {nameof(ISlashCommandContext)} belongs to a message component");
        }

        public async Task<IResult> ExecuteAsync(ISlashCommandContext context, IEnumerable<SlashParameterInfo> paramList,
            IEnumerable<SocketInteractionParameter> argList, IServiceProvider services)
        {
            services = services ?? EmptyServiceProvider.Instance;

            try
            {
                object[] args = GenerateArgs(paramList, argList);

                if (CommandService._runAsync)
                {
                    _ = Task.Run(async ( ) =>
                    {
                        await ExecuteInternalAsync(context, args, services).ConfigureAwait(false);
                    });
                }
                else
                    return await ExecuteInternalAsync(context, args, services).ConfigureAwait(false);

                return ExecuteResult.FromSuccess();
            }
            catch (Exception ex)
            {
                return ExecuteResult.FromError(ex);
            }
        }

        private async Task<IResult> ExecuteInternalAsync ( ISlashCommandContext context, object[] args, IServiceProvider services )
        {
            await Module.CommandService._cmdLogger.DebugAsync($"Executing {GetLogString(context)}").ConfigureAwait(false);

            try
            {
                var task = _action(context, args, services, this);

                if(task is Task<IResult> resultTask)
                {
                    var result = await resultTask.ConfigureAwait(false);
                    await Module.CommandService._commandExecutedEvent.InvokeAsync(this, context, result).ConfigureAwait(false);
                    if (result is RuntimeResult execResult)
                        return execResult;
                }
                else if(task is Task<ExecuteResult> execTask)
                {
                    var result = await execTask.ConfigureAwait(false);
                    await Module.CommandService._commandExecutedEvent.InvokeAsync(this, context, result).ConfigureAwait(false);
                    return result;
                }
                else
                {
                    await task.ConfigureAwait(false);
                    var result = ExecuteResult.FromSuccess();
                    await Module.CommandService._commandExecutedEvent.InvokeAsync(this, context, result).ConfigureAwait(false);
                    return result;
                }

                return ExecuteResult.FromError(SlashCommandError.Unsuccessful, "Command execution failed for an unknown reason");
            }
            catch (Exception ex)
            {
                var originalEx = ex;
                while (ex is TargetInvocationException)
                    ex = ex.InnerException;

                await Module.CommandService._cmdLogger.ErrorAsync(ex);

                var result = ExecuteResult.FromError(ex);
                await Module.CommandService._commandExecutedEvent.InvokeAsync(this, context, result).ConfigureAwait(false);

                if (Module.CommandService._throwOnError)
                {
                    if (ex == originalEx)
                        throw;
                    else
                        ExceptionDispatchInfo.Capture(ex).Throw();
                }

                return result;
            }
            finally
            {
                await Module.CommandService._cmdLogger.VerboseAsync($"Executed {GetLogString(context)}").ConfigureAwait(false);
            }
        }

        private object[] GenerateArgs (IEnumerable<SlashParameterInfo> paramList, IEnumerable<SocketInteractionParameter> argList )
        {
            var args = argList?.ToList();
            var result = new List<object>();

            void AddValue(SocketInteractionParameter param)
            {
                if (param.Value is Optional<object> optional)
                    result.Add(optional.Value);
                else
                    result.Add(param.Value);
            }

            foreach(var parameter in paramList)
            {
                var arg = args?.FirstOrDefault(x => string.Equals(x.Name, parameter.Name, StringComparison.OrdinalIgnoreCase));

                if(arg == null || arg == default)
                {
                    if (parameter.IsRequired)
                        throw new InvalidOperationException("Command was invoked with too few parameters");
                    else
                        result.Add(Type.Missing);
                }
                else
                {
                    if (parameter.Attributes.Any(x => x is ParamArrayAttribute))
                        foreach (var remaining in args)
                            AddValue(remaining);
                    else
                        AddValue(arg);

                    args?.Remove(arg);
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
