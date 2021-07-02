using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Model = Discord.API.ApplicationCommandInteractionData;

namespace Discord.WebSocket
{
    public class SocketInteractionData : SocketEntity<ulong>
    {

        public IReadOnlyCollection<SocketGuildUser> Users { get; } = ImmutableArray.Create<SocketGuildUser>();
        public IReadOnlyCollection<SocketGuildChannel> Channels { get; } = ImmutableArray.Create<SocketGuildChannel>();
        public IReadOnlyCollection<SocketRole> Roles { get; } = ImmutableArray.Create<SocketRole>();
        public IReadOnlyCollection<SocketInteractionParameter> Parameters { get; } = ImmutableArray.Create<SocketInteractionParameter>();

        public DateTimeOffset CreatedAt => SnowflakeUtils.FromSnowflake(Id);

        internal SocketInteractionData (DiscordSocketClient discord, ulong id ,Model model) : base(discord,id)
        {
        }
    }
}
