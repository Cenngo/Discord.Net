using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Discord
{
    public class MessageUI : IReadOnlyList<MessageActionRowComponent>
    {
        private readonly List<MessageActionRowComponent> _rows = new List<MessageActionRowComponent>();
        public MessageActionRowComponent this[int index] => _rows[index];

        public int Count => _rows.Count;

        public IEnumerator<MessageActionRowComponent> GetEnumerator ( ) => _rows.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator ( ) => GetEnumerator();

        internal MessageUI(IEnumerable<IEnumerable<MessageComponent>> components)
        {
            _rows = components.Select(x => new MessageActionRowComponent(x)).ToList();
        }

        internal MessageUI(MessageUIBuilder builder)
        {
            foreach(var row in builder.Rows)
                _rows.Add(new MessageActionRowComponent(row.Cast<MessageComponent>()));
        }
    }
}
