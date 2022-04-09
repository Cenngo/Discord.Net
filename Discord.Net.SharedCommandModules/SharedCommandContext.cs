using Discord.Commands;

namespace Discord.SharedCommandModules
{
    public class SharedCommandContext : ICommandContext, IInteractionContext
    {
        public IDiscordClient Client { get; }

        public IGuild Guild { get; }

        public IMessageChannel Channel { get; }

        public IUser User { get; }

        public IUserMessage Message { get; }

        public IDiscordInteraction Interaction { get; }

        public CommandType CommandType { get; }

        public SharedCommandContext(IDiscordClient client, IUserMessage msg)
        {
            Client = client;
            Guild = (msg.Channel as IGuildChannel)?.Guild;
            Channel = msg.Channel;
            User = msg.Author;
            Message = msg;
            CommandType = CommandType.TextCommand;
        }

        public SharedCommandContext(IDiscordClient client, IDiscordInteraction interaction, IMessageChannel channel = null)
        {
            Client = client;
            Interaction = interaction;
            Channel = channel;
            Guild = (interaction.User as IGuildUser)?.Guild;
            User = interaction.User;
            Interaction = interaction;
            CommandType = CommandType.Interaction;
        }
    }
}
