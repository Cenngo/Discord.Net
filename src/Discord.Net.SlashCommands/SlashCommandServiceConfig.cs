namespace Discord.SlashCommands
{
    public class SlashCommandServiceConfig
    {
        public LogSeverity LogLevel { get; } = LogSeverity.Info;
        public bool RunAsync { get; set; } = true;
        public bool ThrowOnError { get; set; } = true;
        public char InteractionCustomIdDelimiter = ',';
    }
}
