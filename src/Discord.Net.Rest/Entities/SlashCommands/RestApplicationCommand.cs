using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = Discord.API.ApplicationCommand;

namespace Discord.Rest
{
    public class RestApplicationCommand : RestEntity<ulong>, IApplicationCommand, IUpdateable
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public ulong ApplicationId { get; }

        public bool DefaultPermission { get; private set; }

        public RestGuild Guild { get; }
        public bool IsGlobal => Guild == null;

        public IReadOnlyList<IApplicationCommandOption> Options { get; private set; }

        public DateTimeOffset CreatedAt => SnowflakeUtils.FromSnowflake(Id);

        IEnumerable<IApplicationCommandOption> IApplicationCommand.Options => Options;

        IGuild IApplicationCommand.Guild => Guild;

        internal RestApplicationCommand (BaseDiscordClient discord, ulong id, IGuild guild, Model model ) : base(discord, id)
        {
            
        }

        internal void Update ( Model model )
        {
            Name = model.Name;
            Description = model.Description;
            if (model.DefaultPermission.IsSpecified)
                DefaultPermission = model.DefaultPermission.Value;
            if (model.Options.IsSpecified)
                Options = model.Options.Value.Select(x => new ApplicationCommandOption(x)).ToList();
        }

        public async Task UpdateAsync (RequestOptions options = null)
        {
            Model model;

            if (IsGlobal)
                model = await Discord.ApiClient.GetGlobalApplicationCommand(ApplicationId, Id, options).ConfigureAwait(false);
            else
                model = await Discord.ApiClient.GetGuildApplicationCommand(ApplicationId, Guild.Id, Id, options);

            Update(model);
        }
    }
}
