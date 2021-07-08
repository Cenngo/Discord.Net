using Discord.API;
using Discord.API.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discord.Rest
{
    internal static class SlashCommandHelper
    {
        #region Commands
        public static async Task<IEnumerable<RestApplicationCommand>> GetApplicationCommands (BaseDiscordClient discord,
            ulong applicationId, IGuild guild, RequestOptions options)
        {
            IEnumerable<API.ApplicationCommand> commands;

            if (guild != null)
                commands = await discord.ApiClient.GetGuildApplicationCommands(applicationId, guild.Id, options).ConfigureAwait(false);
            else
                commands = await discord.ApiClient.GetGlobalApplicationCommands(applicationId, options).ConfigureAwait(false);

            return commands.Select(x =>
            {
                var restGuild = x.GuildId.IsSpecified ? new RestGuild(discord, x.GuildId.Value) : null;
                return new RestApplicationCommand(discord, x.Id, restGuild, x);
            });
        }

        public static async Task<RestApplicationCommand> GetApplicationCommand (BaseDiscordClient discord, ulong applicationId, IGuild guild,
            ulong commandId, RequestOptions options)
        {
            ApplicationCommand command;

            if (guild != null)
                command = await discord.ApiClient.GetGuildApplicationCommand(applicationId, commandId, commandId, options).ConfigureAwait(false);
            else
                command = await discord.ApiClient.GetGlobalApplicationCommand(applicationId, commandId, options).ConfigureAwait(false);

            var restGuild = command.GuildId.IsSpecified ? new RestGuild(discord, command.GuildId.Value) : null;
            return new RestApplicationCommand(discord, command.Id, restGuild, command);
        }

        public static async Task DeleteApplicationCommand (BaseDiscordClient discord, ulong applicationId, ulong commandId, IGuild guild,
            RequestOptions options)
        {
            if (guild != null)
                await discord.ApiClient.DeleteGuildApplicationCommand(applicationId, guild.Id, commandId, options).ConfigureAwait(false);
            else
                await discord.ApiClient.DeleteGlobalApplicationCommand(applicationId, commandId, options).ConfigureAwait(false);
        }

        public static async Task<RestApplicationCommand> ModifyApplicationCommand (BaseDiscordClient discord, ulong applicationId, ulong commandId,
            IGuild guild, string name, string description, bool defaultPermission, IEnumerable<IApplicationCommandOption> commandOptions, RequestOptions options)
        {
            var args = new ModifyApplicationCommandParams(name, description)
            {
                DefaultPermission = defaultPermission,
                Options = commandOptions.Select(x => x.ToModel()).ToArray()
            };

            API.ApplicationCommand command;

            if (guild != null)
                command = await discord.ApiClient.EditApplicationCommand(applicationId, guild.Id, commandId, args, options).ConfigureAwait(false);
            else
                command = await discord.ApiClient.EditGlobalApplicationCommand(applicationId, commandId, args, options).ConfigureAwait(false);

            var restGuild = command.GuildId.IsSpecified ? new RestGuild(discord, command.GuildId.Value) : null;
            return new RestApplicationCommand(discord, command.Id, restGuild, command);
        }

        public static async Task<ulong> CreateApplicationCommand (BaseDiscordClient discord, ulong applicationId, string name,
            string description, bool defaultPermission, IEnumerable<ApplicationCommandOption> commandOptions, IGuild guild, RequestOptions options)
        {
            var args = new CreateApplicationCommandParams(name, description)
            {
                DefaultPermission = defaultPermission,
                Options = commandOptions.Select(x => x.ToModel()).ToArray()
            };

            ApplicationCommand command;
            if (guild != null)
                command = await discord.ApiClient.CreateGuildApplicationCommand(applicationId, guild.Id, args, options).ConfigureAwait(false);
            else
                command = await discord.ApiClient.CreateGlobalApplicationCommand(applicationId, args, options).ConfigureAwait(false);

            return command.Id;
        }

        public static async Task<IEnumerable<ulong>> BulkOverwriteApplicationCommands (BaseDiscordClient discord, ulong applicationId,
            IEnumerable<IApplicationCommand> commands, IGuild guild, RequestOptions options)
        {
            var models = commands.Select(x => x.ToModel());

            IEnumerable<ApplicationCommand> echoCommands;

            if (guild != null)
                echoCommands = await discord.ApiClient.BulkOverwriteGuildApplicationCommands(applicationId, guild.Id, models, options).ConfigureAwait(false);
            else
                echoCommands = await discord.ApiClient.BulkOverwriteGlobalApplicationCommands(applicationId, models, options).ConfigureAwait(false);

            return echoCommands.Select(x => x.Id);
        }
        #endregion

        #region InteractionResponse
        public static async Task SendInteractionResponse (BaseDiscordClient discord, IDiscordInteraction interaction,
            string text, bool isTTS, IEnumerable<Embed> embeds, AllowedMentions allowedMentions, IEnumerable<MessageComponent> messageComponents,
            InteractionApplicationCommandCallbackFlags flags,
            RequestOptions options)
        {
            Preconditions.AtMost(embeds?.Count() ?? 0, 10, nameof(embeds), "A max of 10 embeds are allowed.");
            CheckAllowedMentions(allowedMentions);

            if (!interaction.Token.IsValid)
                throw new InvalidOperationException("Callback token for this interaction is exprired.");

            string token = interaction.Token.Token;

            var data = new InteractionApplicationCommandCallbackData
            {
                TTS = isTTS,
                Content = text,
                Embeds = embeds?.Select(x => x?.ToModel()).ToArray(),
                AllowedMentions = allowedMentions?.ToModel(),
                Flags = flags,
                Components = messageComponents?.Select(x => x?.ToModel()).ToArray()
            };

            InteractionCallbackType callbackType;
            switch (interaction.InteractionType)
            {
                case InteractionType.Ping:
                    throw new InvalidOperationException($"Interaction type {nameof(InteractionType.Ping)} only supports acknowledgement as a response.");
                case InteractionType.ApplicationCommand:
                    callbackType = InteractionCallbackType.ChannelMessageWithSource;
                    break;
                case InteractionType.MessageComponent:
                    callbackType = InteractionCallbackType.UpdateMessage;
                    break;
                default:
                    throw new ArgumentException($"Interaction type {nameof(interaction.InteractionType)} is not supported for sending a response");
            }

            var args = new CreateInteractionResponseParams(callbackType)
            {
                Data = data
            };
            await discord.ApiClient.CreateInteractionResponse(interaction.Id, token, args, options).ConfigureAwait(false);
        }

        public static async Task SendAcknowledgement(BaseDiscordClient discord, IDiscordInteraction interaction, RequestOptions options)
        {
            if (!interaction.Token.IsValid)
                throw new InvalidOperationException("Callback token for this interaction is exprired.");

            CreateInteractionResponseParams args;
            switch (interaction.InteractionType)
            {
                case InteractionType.Ping:
                    args = new CreateInteractionResponseParams(InteractionCallbackType.Pong);
                    break;
                case InteractionType.ApplicationCommand:
                    args = new CreateInteractionResponseParams(InteractionCallbackType.DeferredChannelMessageWithSource);
                    break;
                case InteractionType.MessageComponent:
                    args = new CreateInteractionResponseParams(InteractionCallbackType.DeferredUpdateMessage);
                    break;
                default:
                    throw new InvalidOperationException("This interaction type is not supported for sending an acknowledgement.");
            }

            await discord.ApiClient.CreateInteractionResponse(interaction.Id, interaction.Token.Token, args, options).ConfigureAwait(false);
        }

        public static async Task<InteractionResponse> GetInteractionResponse (BaseDiscordClient discord, IDiscordInteraction interaction, RequestOptions options)
        {
            if (!interaction.Token.IsValid)
                throw new InvalidOperationException("Callback token for this interaction is exprired.");

            string token = interaction.Token.Token;
            ulong appId = interaction.ApplicationId;

            var response = await discord.ApiClient.GetOriginalInteractionResponse(appId, token, options);

            if (!response.Data.IsSpecified)
                return new InteractionResponse(response.Type);
            else
                return response.ToEntity();
        }

        public static async Task ModifyInteractionResponse (BaseDiscordClient discord, IDiscordInteraction interaction, string text,
            AllowedMentions allowedMentions, IEnumerable<Embed> embeds, IEnumerable<MessageComponent> messageComponents, RequestOptions options)
        {
            Preconditions.AtMost(embeds?.Count() ?? 0, 10, nameof(embeds), "A max of 10 embeds are allowed.");
            CheckAllowedMentions(allowedMentions);

            if (!interaction.Token.IsValid)
                throw new InvalidOperationException("Callback token for this interaction is exprired.");

            string token = interaction.Token.Token;
            ulong appId = interaction.ApplicationId;

            var args = new ModifyInteractionResponseParams()
            {
                Content = text,
                AllowedMentions = allowedMentions?.ToModel(),
                Embeds = embeds?.Select(x => x?.ToModel()).ToArray(),
                Components = messageComponents?.Select(x => x?.ToModel()).ToArray()
            };

            if (args.Components.GetValueOrDefault(null) != null && args.Content.GetValueOrDefault(null) == null)
                throw new InvalidOperationException("Message components must be provided alongside a string content.");

            await discord.ApiClient.EditOriginalInteraction(appId, token, args, options).ConfigureAwait(false);
        }

        public static async Task DeleteInteractionResponse (BaseDiscordClient discord, IDiscordInteraction interaction, RequestOptions options)
        {
            if (!interaction.Token.IsValid)
                throw new InvalidOperationException("Callback token for this interaction is exprired.");

            string token = interaction.Token.Token;
            ulong appId = interaction.ApplicationId;

            await discord.ApiClient.DeleteOriginalInteractionResponse(appId, token, options).ConfigureAwait(false);
        }
        #endregion



        #region Followup
        public static async Task<RestMessage> SendInteractionFollowup (BaseDiscordClient discord, IDiscordInteraction interaction, string text,
            bool isTTS, IEnumerable<Embed> embeds, string username, string avatarUrl, AllowedMentions allowedMentions, RequestOptions options)
        {
            Preconditions.AtMost(embeds?.Count() ?? 0, 10, nameof(embeds), "A max of 10 embeds are allowed.");
            CheckAllowedMentions(allowedMentions);

            if (!interaction.Token.IsValid)
                throw new InvalidOperationException("Callback token for this interaction is exprired.");

            string token = interaction.Token.Token;
            ulong appId = interaction.ApplicationId;

            var args = new CreateWebhookMessageParams(text)
            {
                IsTTS = isTTS,
                Embeds = embeds?.Select(x => x?.ToModel()).ToArray(),
                Username = username,
                AvatarUrl = avatarUrl,
                AllowedMentions = allowedMentions?.ToModel()
            };

            var message = await discord.ApiClient.CreateFollowupMessage(appId, token, args, options).ConfigureAwait(false);
            var user = RestUser.Create(discord, message.Author.GetValueOrDefault(null));
            return RestMessage.Create(discord, interaction.Channel, user, message);
        }

        public static async Task ModifyFollowupMessage (BaseDiscordClient discord, IDiscordInteraction interaction, ulong messageId,
            string text, AllowedMentions allowedMentions, IEnumerable<Embed> embeds, RequestOptions options)
        {
            Preconditions.AtMost(embeds?.Count() ?? 0, 10, nameof(embeds), "A max of 10 embeds are allowed.");
            CheckAllowedMentions(allowedMentions);

            if (!interaction.Token.IsValid)
                throw new InvalidOperationException("Callback token for this interaction is exprired.");

            string token = interaction.Token.Token;
            ulong appId = interaction.ApplicationId;

            var args = new ModifyWebhookMessageParams()
            {
                Content = text,
                Embeds = embeds?.Select(x => x?.ToModel()).ToArray(),
                AllowedMentions = allowedMentions?.ToModel()
            };
            await discord.ApiClient.EditFollowupMessage(appId, token, messageId, args, options).ConfigureAwait(false);
        }

        public static async Task DeleteFollowupMessage (BaseDiscordClient discord, IDiscordInteraction interaction, ulong messageId,
            RequestOptions options)
        {
            if (!interaction.Token.IsValid)
                throw new InvalidOperationException("Callback token for this interaction is exprired.");

            string token = interaction.Token.Token;
            ulong appId = interaction.ApplicationId;

            await discord.ApiClient.DeleteFollowupMessage(appId, token, messageId, options).ConfigureAwait(false);
        }
        #endregion

        // Implement
        #region Permissions
        public static async Task GetGuildCommandPermisions (BaseDiscordClient discord, ulong applicationId, IGuild guild, RequestOptions options)
        {
            var guildId = guild.Id;

            await discord.ApiClient.GetGuildApplicationCommandPermissions(applicationId, guildId, options).ConfigureAwait(false);
        }

        public static async Task GetCommandPermissions (BaseDiscordClient discord, ulong applicationId, IGuild guild)
        {

        }
        #endregion

        #region Utils
        private static void CheckAllowedMentions (AllowedMentions allowedMentions)
        {
            Preconditions.AtMost(allowedMentions?.RoleIds?.Count ?? 0, 100, nameof(allowedMentions.RoleIds), "A max of 100 role Ids are allowed.");
            Preconditions.AtMost(allowedMentions?.UserIds?.Count ?? 0, 100, nameof(allowedMentions.UserIds), "A max of 100 user Ids are allowed.");

            if (allowedMentions != null && allowedMentions.AllowedTypes.HasValue)
            {
                if (allowedMentions.AllowedTypes.Value.HasFlag(AllowedMentionTypes.Users) &&
                    allowedMentions.UserIds != null && allowedMentions.UserIds.Count > 0)
                {
                    throw new ArgumentException("The Users flag is mutually exclusive with the list of User Ids.", nameof(allowedMentions));
                }

                if (allowedMentions.AllowedTypes.Value.HasFlag(AllowedMentionTypes.Roles) &&
                    allowedMentions.RoleIds != null && allowedMentions.RoleIds.Count > 0)
                {
                    throw new ArgumentException("The Roles flag is mutually exclusive with the list of Role Ids.", nameof(allowedMentions));
                }
            }
        }
        #endregion
    }
}
