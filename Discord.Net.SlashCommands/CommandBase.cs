using Discord.SlashCommands.Builders;
using Discord.WebSocket;
using System;

namespace Discord.SlashCommands
{
    public abstract class CommandBase<T> : ISlashModuleBase where T : class, ISlashCommandContext
    {
        public T Context { get; private set; }
        public virtual void AfterExecute (SlashCommandInfo command)
        {
            ( Context.Interaction as SocketCommandInteraction ).DeleteAsync().ConfigureAwait(false);
        }
        public virtual void BeforeExecute (SlashCommandInfo command) { }
        public virtual void OnModuleBuilding (SlashCommandService commandService, SlashModuleBuilder builder) { }
        public virtual void SetContext (ISlashCommandContext context)
        {
            var newValue = context as T;
            Context = newValue ?? throw new InvalidOperationException($"Invalid context type. Expected {typeof(T).Name}, got {context.GetType().Name}.");
        }
    }
}
