using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.API
{
    internal class MessageComponent
    {
        [JsonProperty("type")]
        public MessageComponentType Type { get; set; }
        [JsonProperty("style")]
        public Optional<ButtonStyles> Style { get; set; }
        [JsonProperty("label")]
        public Optional<string> Label { get; set; }
        [JsonProperty("emoji")]
        public Optional<Emoji> Emoji { get; set; }
        [JsonProperty("custom_id")]
        public Optional<string> CustomId { get; set; }
        [JsonProperty("url")]
        public Optional<string> Url { get; set; }
        [JsonProperty("disabled")]
        public Optional<bool> Disabled { get; set; }
        [JsonProperty("components")]
        public Optional<MessageComponent> Components { get; set; }
    }
}
