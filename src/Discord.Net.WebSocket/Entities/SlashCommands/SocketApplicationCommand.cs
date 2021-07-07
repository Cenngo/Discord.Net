using System;
using System.Collections.Generic;
using System.Linq;
using Model = Discord.API.ApplicationCommand;

namespace Discord.WebSocket
{
    public class SocketApplicationCommand : SocketEntity<ulong>, IApplicationCommand
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        public ulong ApplicationId { get; }

        public bool DefaultPermission { get; private set; }

        public SocketGuild Guild { get; }

        public IReadOnlyList<ApplicationCommandOption> Options { get; private set; }

        public DateTimeOffset CreatedAt => SnowflakeUtils.FromSnowflake(Id);

        ulong IApplicationCommand.ApplicationId => ApplicationId;

        IGuild IApplicationCommand.Guild => Guild;

        IEnumerable<IApplicationCommandOption> IApplicationCommand.Options => Options;

        internal SocketApplicationCommand (DiscordSocketClient discord, ulong id, SocketGuild guild, Model model) : base(discord, id)
        {
            Name = model.Name;
            Guild = guild;
            ApplicationId = model.ApplicationId;

            Update(model);
        }

        internal void Update ( Model model )
        {
            Name = model.Name;
            Description = model.Description;

            if(model.DefaultPermission.IsSpecified)
                DefaultPermission = model.DefaultPermission.Value;

            if(model.Options.IsSpecified)
                Options = model.Options.Value.Select(x => new ApplicationCommandOption(x, null)).ToList();
        }
    }
}
