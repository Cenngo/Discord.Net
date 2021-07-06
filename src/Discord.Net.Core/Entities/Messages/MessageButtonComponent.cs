using System;

namespace Discord
{
    public class MessageButtonComponent : MessageComponent, IDiscordInteractable
    {
        public ButtonStyles Style { get; }
        public string Label { get; }
        public Emote? Emoji { get; }
        public string CustomId { get; }
        public string Url { get; }
        public bool IsDisabled { get; } = false;

        internal MessageButtonComponent (string label = null, string customId = null, string url = null,
            Emote emoji = null, ButtonStyles style = ButtonStyles.Primary, bool isDisabled = false) : base(MessageComponentType.Button)
        {
            if (string.IsNullOrEmpty(label) && string.IsNullOrEmpty(customId))
                throw new ArgumentException($"Either one of {nameof(label)} or {nameof(customId)} must be set.");

            Label = label;
            CustomId = customId;
            Url = url;
            Emoji = emoji;
            Style = style;
            IsDisabled = isDisabled;
        }

        internal MessageButtonComponent (ButtonBuilder builder) : base(MessageComponentType.Button)
        {
            if (string.IsNullOrEmpty(builder.Label) && string.IsNullOrEmpty(builder.CustomId))
                throw new ArgumentException($"Either one of {nameof(builder.Label)} or {nameof(builder.CustomId)} must be set.");

            Label = builder.Label;
            CustomId = builder.CustomId;
            Url = builder.Url;
            Emoji = builder.Emoji;
            IsDisabled = builder.IsDisabled;
            Style = builder.Style;
        }

        public override string ToString ( ) => Label;
    }
}
