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

        Task AcknowledgeAsync ( );
        Task SendFollowupAsync ( );
        Task SendResponseAsync ( );
    }
}
