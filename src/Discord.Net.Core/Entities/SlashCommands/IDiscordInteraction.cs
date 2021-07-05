using System.Threading.Tasks;

namespace Discord
{
    public interface IDiscordInteraction : IDeletable, ISnowflakeEntity
    {
        InteractionType InteractionType { get; }
        IUser User { get; }
        IMessageChannel Channel { get; }
        IDiscordInteractionToken Token { get; }
        int Version { get; }
        internal ulong ApplicationId { get; }
    }
}
