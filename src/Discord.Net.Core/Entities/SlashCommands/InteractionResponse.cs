using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Discord
{
    public class InteractionResponse
    {
        public bool IsTTS { get; internal set; }
        public string Content { get; internal set; }
        public InteractionCallbackType CallbackType { get; internal set; }
        public IReadOnlyCollection<Embed> Embeds { get; internal set; }
        public AllowedMentions AllowedMentions { get; internal set; }
        public IReadOnlyCollection<MessageComponent> MessageComponents { get; internal set; }

        internal InteractionResponse (bool isTTs, string content, InteractionCallbackType type, IEnumerable<Embed> embeds,
            AllowedMentions allowedMentions, IEnumerable<MessageComponent> messageComponents)
        {
            IsTTS = isTTs;
            Content = content;
            CallbackType = type;
            Embeds = embeds.ToImmutableArray();
            AllowedMentions = allowedMentions;
            MessageComponents = messageComponents.ToImmutableArray();
        }

        internal InteractionResponse(InteractionCallbackType type)
        {
            CallbackType = type;
        }
    }
}
