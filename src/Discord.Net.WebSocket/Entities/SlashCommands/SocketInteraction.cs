using Discord.API;
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

        IUser IDiscordInteraction.User => User;
        IDiscordInteractionToken IDiscordInteraction.Token => Token;
        IMessageChannel IDiscordInteraction.Channel => Channel;

        internal SocketInteraction (DiscordSocketClient discord, ClientState state, SocketUser user, ISocketMessageChannel channel, Model model)
            : base(discord, model.Id)
        {
            User = user;
            InteractionType = model.Type;
            Version = model.Version;
            Channel = channel;
            Token = new SocketInteractionToken(model.Token, model.Id);
        }

        internal static SocketInteraction Create(DiscordSocketClient discord, ClientState state, SocketUser user, ISocketMessageChannel channel, Model model)
        {
            if (model.Type == InteractionType.ApplicationCommand || model.Type == InteractionType.MessageComponent)
                return new SocketCommandInteraction(discord, state, user, channel, model);
            else
                return new SocketMessageInteraction(discord, state, user, channel, model);
        }

        internal virtual void Update (ClientState state, Model model )
        {
        }

        public Task DeleteAsync (RequestOptions options = null) => throw new NotImplementedException();
        public Task AcknowledgeAsync ( ) => throw new NotImplementedException();
        public Task SendFollowupAsync ( ) => throw new NotImplementedException();
        public Task SendResponseAsync ( ) => throw new NotImplementedException();
    }
}
