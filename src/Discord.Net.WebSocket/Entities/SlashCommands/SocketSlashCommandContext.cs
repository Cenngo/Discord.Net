using Discord.WebSocket;

namespace Discord.WebSocket
{
    public class SocketSlashCommandContext : ISlashCommandContext
    {
        public DiscordSocketClient Client { get; }
        public SocketGuild Guild { get; }
        public ISocketMessageChannel Channel { get; }
        public SocketUser User { get; }
        public SocketInteraction Interaction { get; }

        IDiscordClient ISlashCommandContext.Client => Client;

        IGuild ISlashCommandContext.Guild => Guild;

        IMessageChannel ISlashCommandContext.Channel => Channel;

        IUser ISlashCommandContext.User => User;

        IDiscordInteraction ISlashCommandContext.Interaction => Interaction;

        public SocketSlashCommandContext
            (DiscordSocketClient client, SocketInteraction interaction)
        {
            Client = client;
            Channel = interaction.Channel;
            Guild = ( interaction.User as SocketGuildUser )?.Guild;
            User = interaction.User;
            Interaction = interaction;
        }
    }
}
