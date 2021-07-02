using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.SlashCommands
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class GuildAttribute : Attribute
    {
        public ulong GuildId { get; }

        public GuildAttribute(ulong guildId)
        {
            GuildId = guildId;
        }
    }
}
