using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.SlashCommands
{
    public class SlashCommandContext : ISlashCommandContext
    {
        public virtual IDiscordClient Client { get; }
        public virtual IGuild Guild { get; }
        public virtual IMessageChannel Channel { get; }
        public virtual IUser User { get; }
        public virtual IDiscordInteraction Interaction { get; }
        public bool IsPrivate => Channel is IPrivateChannel;

        public SlashCommandContext ( IDiscordClient client, IDiscordInteraction interaction )
        {
            Client = client;
            Channel = interaction.Channel;
            Guild = ( interaction.Channel as IGuildChannel )?.Guild;
            User = interaction.User;
            Interaction = interaction;
        }
    }
}
