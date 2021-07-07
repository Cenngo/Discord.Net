using System.Threading.Tasks;

namespace Discord
{
    /// <summary>
    /// Represents an Discord Client originated user interaction
    /// </summary>
    public interface IDiscordInteraction : IDeletable, ISnowflakeEntity
    {
        /// <summary>
        /// Get the type of the interaction
        /// </summary>
        InteractionType InteractionType { get; }
        /// <summary>
        /// Get the user that created this interaction
        /// </summary>
        IUser User { get; }
        /// <summary>
        /// Get the channel this interaction originated from
        /// </summary>
        IMessageChannel Channel { get; }
        /// <summary>
        /// Get the manipulation token for this interaction
        /// </summary>
        /// <remarks>
        /// Valid for 15 mins
        /// </remarks>
        IDiscordInteractionToken Token { get; }
        /// <summary>
        /// Get the version of the Interaction API
        /// </summary>
        /// <remarks>
        /// Constant 1
        /// </remarks>
        int Version { get; }
        /// <summary>
        /// Get the Snowflake ID of the application this interaction was issued to
        /// </summary>
        internal ulong ApplicationId { get; }
    }
}
