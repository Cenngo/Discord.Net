using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    public class MessageSelectMenuComponent : MessageComponent, IDiscordInteractable
    {
        public string CustomId { get; }
        public string Placeholder { get; }
        public int MinValues { get; }
        public int MaxValues { get; }
        public IReadOnlyList<SelectOption> Options { get; }

        internal MessageSelectMenuComponent (string customid, IEnumerable<SelectOption> options, string placeholder = null, int min = 1, int max = 1 )
            : base(MessageComponentType.SelectMenu)
        {
            CustomId = customid;
            Placeholder = placeholder;
            Options = options.ToImmutableArray();
            Placeholder = placeholder;
            MinValues = min;
            MaxValues = max;
        }

        internal MessageSelectMenuComponent(SelectMenuBuilder builder) : base(MessageComponentType.SelectMenu)
        {
            CustomId = builder.CustomId;
            Placeholder = builder.Placeholder;
            Options = builder.Options.ToImmutableArray();
            MinValues = builder.MinValues;
            MaxValues = builder.MaxValues;
        }
    }

    public class SelectOption
    {
        public string Label { get; }
        public string Value { get; }
        public string Description { get; }
        public Emote Emoji { get; }
        public bool IsDefault { get; }

        public SelectOption(string label, string value, string description = null, Emote emoji = null, bool isDefault = false)
        {
            Label = label;
            Value = value;
            Description = description;
            Emoji = emoji;
            IsDefault = isDefault;
        }
    }
}
