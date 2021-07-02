using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.API
{
    internal class ApplicationCommandPermission
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("type")]
        public ApplicationCommandPermissionType Type { get; set; }
        [JsonProperty("permission")]
        public bool Allow { get; set; }
    }
}
