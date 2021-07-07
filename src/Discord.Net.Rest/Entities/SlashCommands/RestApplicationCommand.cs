using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model = Discord.API.ApplicationCommand;

namespace Discord.Rest
{
    /// <summary>
    /// Represents a REST-based Application Command
    /// </summary>
    public class RestApplicationCommand : RestEntity<ulong>, IApplicationCommand, IUpdateable
    {
        /// <inheritdoc/>
        public string Name { get; private set; }
        /// <inheritdoc/>
        public string Description { get; private set; }
        /// <inheritdoc/>
        public ulong ApplicationId { get; }
        /// <inheritdoc/>
        public bool DefaultPermission { get; private set; }
        /// <summary>
        /// Get the guild this command belongs to if the command is a Guild Command
        /// </summary>
        public RestGuild Guild { get; }
        public bool IsGlobal => Guild == null;

        public IReadOnlyList<IApplicationCommandOption> Options { get; private set; }

        public DateTimeOffset CreatedAt => SnowflakeUtils.FromSnowflake(Id);

        IEnumerable<IApplicationCommandOption> IApplicationCommand.Options => Options;

        IGuild IApplicationCommand.Guild => Guild;

        internal RestApplicationCommand (BaseDiscordClient discord, ulong id, IGuild guild, Model model) : base(discord, id)
        {
            if (guild != null)
                Guild = new RestGuild(discord, guild.Id);
            ApplicationId = model.ApplicationId;

            Update(model);
        }

        internal void Update (Model model)
        {
            Name = model.Name;
            Description = model.Description;
            if (model.DefaultPermission.IsSpecified)
                DefaultPermission = model.DefaultPermission.Value;
            if (model.Options.IsSpecified)
                Options = model.Options.Value.Select(x => ApplicationCommandOption.Create(x, ApplicationCommandOption.MaxOptionDepth)).ToList();
        }

        /// <inheritdoc/>
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
