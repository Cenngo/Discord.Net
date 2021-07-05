using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Model = Discord.API.Interaction;

namespace Discord.WebSocket
{
    public class SocketCommandInteraction : SocketInteraction
    {
        public string Command { get; }
        public IReadOnlyCollection<SocketInteractionParameter> Data { get; private set; }

        internal SocketCommandInteraction (DiscordSocketClient discord, ClientState state, SocketUser user, ISocketMessageChannel channel, Model model)
            : base(discord, state, user, channel, model)
        {
            Data = ParseParameters(model, out string command).ToImmutableArray();
            Command = command;
        }

        private IEnumerable<SocketInteractionParameter> ParseParameters (Model model, out string command, string commandDelimiter = " ")
        {
            if (!model.Data.IsSpecified)
                throw new ArgumentException($"Provided Interaction Command Model is not a type of {nameof(SocketCommandInteraction)}");

            var data = model.Data.Value;

            command = data.Name;

            if (!data.Options.IsSpecified)
                return Enumerable.Empty<SocketInteractionParameter>();

            var result = new List<SocketInteractionParameter>();
            var children = data.Options.Value;

            do
            {
                foreach (var option in children)
                {
                    var type = option.Type;

                    if (type == ApplicationCommandOptionType.SubCommand || type == ApplicationCommandOptionType.SubCommandGroup)
                    {
                        command += commandDelimiter + option.Name;
                        children = option.Options.IsSpecified ? option.Options.Value : null;
                    }
                    else
                    {
                        children = null;

                        object value = option.Value;

                        if (ulong.TryParse(value.ToString(), out var id) && data.Resolved.IsSpecified)
                        {
                            var resolved = data.Resolved.Value;

                            switch (option.Type)
                            {
                                case ApplicationCommandOptionType.User:
                                    {
                                        var user = resolved?.Users.Value?[id];

                                        if (User is SocketGuildUser guildUser)
                                        {
                                            var member = resolved?.Members.Value?[id];
                                            if (member != null)
                                                value = guildUser.Guild.AddOrUpdateUser(user);
                                        }
                                        else if (user != null)
                                            value = Discord.GetUser(user.Id);
                                    }
                                    break;
                                case ApplicationCommandOptionType.Role:
                                    {
                                        var role = resolved?.Roles.Value?[id];
                                        if (role != null)
                                            value = ( User as SocketGuildUser ).Guild.GetRole(role.Id);
                                    }
                                    break;
                                case ApplicationCommandOptionType.Channel:
                                    {
                                        var channel = resolved?.Channels.Value?[id];
                                        if (channel != null)
                                            value = Discord.GetChannel(channel.Id);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        result.Add(new SocketInteractionParameter(option.Name, value, SlashCommandUtility.GetParameterType(option.Type)));
                    }
                }
            } while (children != null);
            return result;
        }
    }
}
