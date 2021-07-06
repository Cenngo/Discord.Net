using Discord.API;
using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = Discord.API.Interaction;

namespace Discord.WebSocket
{
    public abstract class SocketInteraction : SocketEntity<ulong>, IDiscordInteraction

    {
        public SocketUser User { get; }
        public DateTimeOffset CreatedAt => SnowflakeUtils.FromSnowflake(Id);
        public InteractionType InteractionType { get; }
        public ISocketMessageChannel Channel { get; }
        public int Version { get; }
        public SocketInteractionToken Token { get; }
        internal ulong ApplicationId { get; }

        IUser IDiscordInteraction.User => User;
        IDiscordInteractionToken IDiscordInteraction.Token => Token;
        IMessageChannel IDiscordInteraction.Channel => Channel;
        ulong IDiscordInteraction.ApplicationId => ApplicationId;

        internal SocketInteraction (DiscordSocketClient discord, ClientState state, SocketUser user, ISocketMessageChannel channel, Model model)
            : base(discord, model.Id)
        {
            User = user;
            InteractionType = model.Type;
            Version = model.Version;
            Channel = channel;
            Token = new SocketInteractionToken(model.Token, model.Id);
            ApplicationId = model.ApplicationId;
        }

        internal static SocketInteraction Create(DiscordSocketClient discord, ClientState state, SocketUser user, ISocketMessageChannel channel, Model model)
        {
            if (model.Type == InteractionType.ApplicationCommand)
                return new SocketCommandInteraction(discord, state, user, channel, model);
            else if (model.Type == InteractionType.MessageComponent)
                return new SocketMessageInteraction(discord, state, user, channel, model);
            else
                throw new ArgumentException("This kind of interaction is not supported.");
        }

        internal virtual void Update (ClientState state, Model model )
        {
        }

        public async Task DeleteAsync (RequestOptions options = null) =>
            await SlashCommandHelper.DeleteInteractionResponse(Discord, this, null).ConfigureAwait(false);
        public async Task AcknowledgeAsync ( RequestOptions options = null) =>
            await SlashCommandHelper.SendAcknowledgement(Discord, this, options).ConfigureAwait(false);

        public async Task SendFollowupAsync (string text = null, bool isTTS = false, string username = null, string avatarUrl = null, IEnumerable<Embed> embeds = null,
            AllowedMentions allowedMentions = null, RequestOptions options = null) =>
            await SlashCommandHelper.SendInteractionFollowup(Discord, this, text, isTTS, embeds, username, avatarUrl, allowedMentions, options);

        public async Task PopulateAcknowledgement ( string text = null, bool isTTS = false, IEnumerable<Embed> embeds = null, AllowedMentions allowedMentions = null,
            InteractionApplicationCommandCallbackFlags flags = 0, IEnumerable<MessageComponent> messageComponents = null, RequestOptions options = null)
        {
            await SlashCommandHelper.ModifyInteractionResponse(Discord, this, text, allowedMentions, embeds, messageComponents, options).ConfigureAwait(false);
        }
    }
}
