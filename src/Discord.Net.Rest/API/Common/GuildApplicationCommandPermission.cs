using Newtonsoft.Json;

namespace Discord.API
{
    internal class GuildApplicationCommandPermission : ApplicationCommandPermission
    {
        [JsonProperty("application_id")]
        public ulong ApplicationId { get; set; }
        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }
        [JsonProperty("permissions")]
        public ApplicationCommandPermission[] Permissions { get; set; }
    }
}
