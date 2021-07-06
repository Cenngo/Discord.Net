using System.Collections.Generic;
using System.Collections.Immutable;

namespace Discord
{
    public class MessageActionRowComponent : MessageComponent
    {
        public IReadOnlyList<MessageComponent> MessageComponents { get; }

        internal MessageActionRowComponent ( IEnumerable<MessageComponent> childComponents ) : base(MessageComponentType.ActionRow)
        {
            MessageComponents = childComponents.ToImmutableArray();
        }
    }
}
