using System;

namespace Discord.WebSocket
{
    public class SocketInteractionToken : IDiscordInteractionToken
    {
        private TimeSpan TTL => TimeSpan.FromMinutes(15);

        public string Token { get; }
        public DateTimeOffset CreatedAt { get; }
        public bool IsValid => CreatedAt + TTL > DateTimeOffset.Now;

        internal SocketInteractionToken (string token, ulong snowflake )
        {
            Token = token;
            CreatedAt = SnowflakeUtils.FromSnowflake(snowflake);
        }

        public override string ToString ( ) => Token;
    }
}
