using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.SlashCommands
{
    public class SocketSlashCommandContext : SlashCommandContext
    {
        public new DiscordSocketClient Client { get; }
        public SocketSlashCommandContext (DiscordSocketClient discord, SocketInteraction interaction ) : base(discord, interaction)
        {

        }
    }
}
