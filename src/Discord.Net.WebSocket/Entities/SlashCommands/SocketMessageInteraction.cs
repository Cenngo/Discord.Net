using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = Discord.API.Interaction;

namespace Discord.WebSocket
{
    public class SocketMessageInteraction : SocketInteraction
    {
        public IReadOnlyCollection<SocketInteractionParameter> Data { get; private set; }
        public SocketMessage Message { get; private set; }
        public MessageComponentType ComponentType { get; }
        public string CustomId { get; }
        public IEnumerable<string> Values { get; }

        internal SocketMessageInteraction (DiscordSocketClient discord, ClientState state, SocketUser user, ISocketMessageChannel channel, Model model)
            : base(discord, state, user, channel, model)
        {
            Message = model.Message.IsSpecified ? Message : null;
            if (model.Data.IsSpecified)
            {
                var data = model.Data.Value;

                ComponentType = data.ComponentType;
                CustomId = data.CustomId;

                if (data.ComponentType == MessageComponentType.SelectMenu)
                    Values = data.Values.GetValueOrDefault(null)?.ToList();
            }
        }
    }
}
