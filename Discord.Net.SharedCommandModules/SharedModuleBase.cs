using Discord.Commands;
using Discord.Interactions;
using Discord.Interactions.Builders;

namespace Discord.SharedCommandModules
{
    public abstract class SharedModuleBase<T> : IInteractionModuleBase, IModuleBase
        where T : SharedCommandContext
    {
        public T Context { get; private set; }

        public void SetContext(T context) =>
            Context = context;

        public virtual void AfterTextCommandExecute(CommandInfo command) { }
        public virtual void BeforeTextCommandExeute(CommandInfo command) { }
        public virtual void OnTextModuleBuilding(CommandService commandService, Commands.Builders.ModuleBuilder builder) { }

        public virtual void AfterInteractionExecute(ICommandInfo command) { }
        public virtual Task AfterInteractionExecuteAsync(ICommandInfo command) => Task.CompletedTask;
        public virtual void BeforeInteractionExecute(ICommandInfo command) { }
        public virtual Task BeforeInterationExecuteAsync(ICommandInfo command) => Task.CompletedTask;
        public virtual void OnInteractionModuleBuilding(InteractionService commandService, Interactions.ModuleInfo module) { }
        public virtual void InteractionModuleConstruct(ModuleBuilder builder, InteractionService commandService) { }

        /// <inheritdoc cref="IDiscordInteraction.DeferAsync(bool, RequestOptions)"/>
        protected virtual async Task DeferAsync(bool ephemeral = false, RequestOptions options = null) =>
            await Context.Interaction.DeferAsync(ephemeral, options).ConfigureAwait(false);

        /// <inheritdoc cref="IDiscordInteraction.RespondAsync(string, Embed[], bool, bool, AllowedMentions, RequestOptions, MessageComponent, Embed)"/>
        protected virtual async Task RespondAsync(string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
            AllowedMentions allowedMentions = null, RequestOptions options = null, MessageComponent components = null, Embed embed = null) =>
            await Context.Interaction.RespondAsync(text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options).ConfigureAwait(false);

        /// <inheritdoc cref="IDiscordInteraction.RespondWithFileAsync(Stream, string, string, Embed[], bool, bool, AllowedMentions, MessageComponent, Embed, RequestOptions)"/>
        protected virtual Task RespondWithFileAsync(Stream fileStream, string fileName, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
            AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null)
            => Context.Interaction.RespondWithFileAsync(fileStream, fileName, text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options);

        /// <inheritdoc cref="IDiscordInteraction.RespondWithFileAsync(string, string, string, Embed[], bool, bool, AllowedMentions, MessageComponent, Embed, RequestOptions)"/>
        protected virtual Task RespondWithFileAsync(string filePath, string fileName = null, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
            AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null)
            => Context.Interaction.RespondWithFileAsync(filePath, fileName, text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options);

        /// <inheritdoc cref="IDiscordInteraction.RespondWithFileAsync(FileAttachment, string, Embed[], bool, bool, AllowedMentions, MessageComponent, Embed, RequestOptions)"/>
        protected virtual Task RespondWithFileAsync(FileAttachment attachment, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
            AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null)
            => Context.Interaction.RespondWithFileAsync(attachment, text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options);

        /// <inheritdoc cref="IDiscordInteraction.RespondWithFilesAsync(IEnumerable{FileAttachment}, string, Embed[], bool, bool, AllowedMentions, MessageComponent, Embed, RequestOptions)"/>
        protected virtual Task RespondWithFilesAsync(IEnumerable<FileAttachment> attachments, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
            AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null)
            => Context.Interaction.RespondWithFilesAsync(attachments, text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options);

        /// <inheritdoc cref="IDiscordInteraction.FollowupAsync(string, Embed[], bool, bool, AllowedMentions, RequestOptions, MessageComponent, Embed)"/>
        protected virtual async Task<IUserMessage> FollowupAsync(string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
            AllowedMentions allowedMentions = null, RequestOptions options = null, MessageComponent components = null, Embed embed = null) =>
            await Context.Interaction.FollowupAsync(text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options).ConfigureAwait(false);

        /// <inheritdoc cref="IDiscordInteraction.FollowupWithFileAsync(Stream, string, string, Embed[], bool, bool, AllowedMentions, MessageComponent, Embed, RequestOptions)"/>
        protected virtual Task<IUserMessage> FollowupWithFileAsync(Stream fileStream, string fileName, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
            AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null)
            => Context.Interaction.FollowupWithFileAsync(fileStream, fileName, text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options);

        /// <inheritdoc cref="IDiscordInteraction.FollowupWithFileAsync(string, string, string, Embed[], bool, bool, AllowedMentions, MessageComponent, Embed, RequestOptions)"/>
        protected virtual Task<IUserMessage> FollowupWithFileAsync(string filePath, string fileName = null, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
            AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null)
            => Context.Interaction.FollowupWithFileAsync(filePath, fileName, text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options);

        /// <inheritdoc cref="IDiscordInteraction.FollowupWithFileAsync(FileAttachment, string, Embed[], bool, bool, AllowedMentions, MessageComponent, Embed, RequestOptions)"/>
        protected virtual Task<IUserMessage> FollowupWithFileAsync(FileAttachment attachment, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
            AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null)
            => Context.Interaction.FollowupWithFileAsync(attachment, text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options);

        /// <inheritdoc cref="IDiscordInteraction.FollowupWithFilesAsync(IEnumerable{FileAttachment}, string, Embed[], bool, bool, AllowedMentions, MessageComponent, Embed, RequestOptions)"/>
        protected virtual Task<IUserMessage> FollowupWithFilesAsync(IEnumerable<FileAttachment> attachments, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
            AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null)
            => Context.Interaction.FollowupWithFilesAsync(attachments, text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options);

        /// <inheritdoc cref="IMessageChannel.SendMessageAsync(string, bool, Embed, RequestOptions, AllowedMentions, MessageReference, MessageComponent, ISticker[], Embed[])"/>
        protected virtual async Task<IUserMessage> ReplyAsync(string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null,
            AllowedMentions allowedMentions = null, MessageReference messageReference = null, MessageComponent components = null) =>
            await Context.Channel.SendMessageAsync(text, false, embed, options, allowedMentions, messageReference, components).ConfigureAwait(false);

        /// <inheritdoc cref="IDiscordInteraction.GetOriginalResponseAsync(RequestOptions)"/>
        protected virtual Task<IUserMessage> GetOriginalResponseAsync(RequestOptions options = null)
            => Context.Interaction.GetOriginalResponseAsync(options);

        /// <inheritdoc cref="IDiscordInteraction.ModifyOriginalResponseAsync(Action{MessageProperties}, RequestOptions)"/>
        protected virtual Task<IUserMessage> ModifyOriginalResponseAsync(Action<MessageProperties> func, RequestOptions options = null)
            => Context.Interaction.ModifyOriginalResponseAsync(func, options);

        /// <inheritdoc cref="IDeletable.DeleteAsync(RequestOptions)"/>
        protected virtual async Task DeleteOriginalResponseAsync()
        {
            var response = await Context.Interaction.GetOriginalResponseAsync().ConfigureAwait(false);
            await response.DeleteAsync().ConfigureAwait(false);
        }

        /// <inheritdoc cref="IDiscordInteraction.RespondWithModalAsync(Modal, RequestOptions)"/>
        protected virtual async Task RespondWithModalAsync(Modal modal, RequestOptions options = null) => await Context.Interaction.RespondWithModalAsync(modal);

        /// <inheritdoc cref="IDiscordInteractionExtentions.RespondWithModalAsync(IDiscordInteraction, IModal, RequestOptions)"/>
        protected virtual async Task RespondWithModalAsync<T>(string customId, RequestOptions options = null) where T : class, IModal
            => await Context.Interaction.RespondWithModalAsync<T>(customId, options);

        //IModuleBase
        void IModuleBase.BeforeExecute(CommandInfo command) => BeforeTextCommandExeute(command);
        void IModuleBase.AfterExecute(CommandInfo command) => AfterTextCommandExecute(command);
        void IModuleBase.OnModuleBuilding(CommandService commandService, Commands.Builders.ModuleBuilder builder) => OnTextModuleBuilding(commandService, builder);
        void IModuleBase.SetContext(ICommandContext context) =>
            Context = context as T ?? throw new InvalidOperationException($"Invalid context type. Expected {typeof(T).Name}, got {context.GetType().Name}.");

        //IInteractionModuleBase
        Task IInteractionModuleBase.BeforeExecuteAsync(ICommandInfo command) => BeforeInterationExecuteAsync(command);
        void IInteractionModuleBase.BeforeExecute(ICommandInfo command) => BeforeInteractionExecute(command);
        Task IInteractionModuleBase.AfterExecuteAsync(ICommandInfo command) => AfterInteractionExecuteAsync(command);
        void IInteractionModuleBase.AfterExecute(ICommandInfo command) => AfterInteractionExecute(command);
        void IInteractionModuleBase.OnModuleBuilding(InteractionService commandService, Interactions.ModuleInfo module) => OnInteractionModuleBuilding(commandService, module);
        void IInteractionModuleBase.Construct(ModuleBuilder builder, InteractionService commandService) => InteractionModuleConstruct(builder, commandService);
        void IInteractionModuleBase.SetContext(IInteractionContext context) =>
            Context = context as T ?? throw new InvalidOperationException($"Invalid context type. Expected {typeof(T).Name}, got {context.GetType().Name}.");
    }
}
