namespace Discord
{
    /// <summary>
    /// Parameter types that are supported by the Application Commands API
    /// </summary>
    public enum ApplicationCommandOptionType
    {
        SubCommand = 1,
        SubCommandGroup = 2,
        String = 3,
        Integer = 4,
        Boolean = 5,
        User = 6,
        Channel = 7,
        Role = 8,
        /// <remarks>
        /// Since all of the complex object types supported by Application Commands API are <see cref="IMentionable"/>s,
        /// this type of parameter is only registered either when it is explicitly declared, or as fallback
        /// </remarks>
        Mentionable = 9
    }
}
