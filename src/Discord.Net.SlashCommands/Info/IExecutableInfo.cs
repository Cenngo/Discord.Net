namespace Discord.SlashCommands
{
    internal interface IExecutableInfo
    {
        string Name { get; }
        SlashModuleInfo Module { get; }
        SlashGroupInfo Group { get; }
    }
}
