namespace Discord
{
    public interface ISlashCommandContext
    {
        IDiscordClient Client { get; }
        IGuild Guild { get; }
        IMessageChannel Channel { get; }
        IUser User { get; }
        IDiscordInteraction Interaction { get; }
    }
}
