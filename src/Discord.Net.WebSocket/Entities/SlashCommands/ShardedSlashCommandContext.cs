using Discord.WebSocket;

namespace Discord.WebSocket
{
    public class ShardedSlashCommandContext : SocketSlashCommandContext, ISlashCommandContext
    {
        public new DiscordShardedClient Client { get; }

        public ShardedSlashCommandContext (DiscordShardedClient client, SocketInteraction interaction)
            : base(client.GetShard(GetShardId(client, ( interaction.User as SocketGuildUser )?.Guild)), interaction)
        {
            Client = client;
        }

        private static int GetShardId (DiscordShardedClient client, IGuild guild)
            => guild == null ? 0 : client.GetShardIdFor(guild);
    }
}
