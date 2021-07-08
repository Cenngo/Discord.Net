using System.Collections.Generic;
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

        /// <summary>
        /// Send an acknowledgement to verify the handoff
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        Task AcknowledgeAsync (RequestOptions options);

        /// <summary>
        /// Send a response that will remove the thinking animation from the original acknowledgement and displayed to the user
        /// </summary>
        /// <param name="text"></param>
        /// <param name="isTTS"></param>
        /// <param name="embeds"></param>
        /// <param name="allowedMentions"></param>
        /// <param name="flags"></param>
        /// <param name="messageComponents"></param>
        /// <param name="options">The options to be used when sending the request</param>
        /// <returns></returns>
        Task PopulateAcknowledgement (string text, bool isTTS, IEnumerable<Embed> embeds, AllowedMentions allowedMentions,
            InteractionApplicationCommandCallbackFlags flags, IEnumerable<MessageComponent> messageComponents, RequestOptions options);

        /// <summary>
        /// Send a response that will be directly shown to the user without displaying the "Thinking" animation
        /// </summary>
        /// <remarks>
        /// If this method is preferred over the "Acknowledge, Modify" method, response must be sent right away for the interaction hand-off to be successful,
        /// otherwise an "interction failed" message will be displayed to the user.
        /// </remarks>
        /// <param name="text"></param>
        /// <param name="isTTS"></param>
        /// <param name="embeds"></param>
        /// <param name="allowedMentions"></param>
        /// <param name="flags"></param>
        /// <param name="messageComponents"></param>
        /// <param name="options">The options to be used when sending the request</param>
        /// <returns></returns>
        Task SendResponse (string text, bool isTTS, IEnumerable<Embed> embeds, AllowedMentions allowedMentions,
            InteractionApplicationCommandCallbackFlags flags, IEnumerable<MessageComponent> messageComponents, RequestOptions options);

        /// <summary>
        /// Delete the Interaction Response
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        new Task DeleteResponse (RequestOptions options);

        /// <summary>
        /// Send a followup message for this interaction
        /// </summary>
        /// <param name="text"></param>
        /// <param name="isTTS"></param>
        /// <param name="username"></param>
        /// <param name="avatarUrl"></param>
        /// <param name="embeds"></param>
        /// <param name="allowedMentions"></param>
        /// <param name="options">The options to be used when sending the request</param>
        /// <returns></returns>
        Task<IMessage> SendFollowupAsync (string text, bool isTTS, string username, string avatarUrl, IEnumerable<Embed> embeds,
            AllowedMentions allowedMentions, RequestOptions options);

        /// <summary>
        /// Modify an Interaction Followup message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="text"></param>
        /// <param name="embeds"></param>
        /// <param name="allowedMentions"></param>
        /// <param name="options">The options to be used when sending the request</param>
        /// <returns></returns>
        Task ModifyFollowup (ulong messageId, string text, IEnumerable<Embed> embeds, AllowedMentions allowedMentions, RequestOptions options);

        /// <summary>
        /// Delete an Interaction Followup message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="options">The options to be used when sending the request</param>
        /// <returns></returns>
        Task DeleteFollowup (ulong messageId, RequestOptions options);
    }
}
