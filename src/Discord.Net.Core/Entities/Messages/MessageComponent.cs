namespace Discord
{
    public abstract class MessageComponent : IMessageComponent
    {
        public MessageComponentType ComponentType { get; }

        internal MessageComponent (MessageComponentType type)
        {
            ComponentType = type;
        }
    }
}
