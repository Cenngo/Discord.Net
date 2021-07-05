using System;
using System.Collections.Generic;
using System.Text;
using Discord.API;
using Discord.API.Rest;
using Discord.Rest;
using Discord.WebSocket;

namespace Discord.SlashCommands
{
    public static class RestHelper
    {
        public static void SendInteractionResponse ( DiscordSocketClient discord, IDiscordInteraction interaction,  InteractionCallbackType type)
        {
            if (!interaction.Token.IsValid)
                throw new InvalidOperationException("Callback token for this interaction is expired.");

            string token = interaction.Token.Token;
        }
    }
}
