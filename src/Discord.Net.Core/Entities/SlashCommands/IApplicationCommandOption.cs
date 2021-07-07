using System.Collections.Generic;

namespace Discord
{
    /// <summary>
    /// Represents an Application Command Option
    /// </summary>
    public interface IApplicationCommandOption
    {
        /// <summary>
        /// Get the type of this option
        /// </summary>
        ApplicationCommandOptionType OptionType { get; }
        /// <summary>
        /// Get the name of this option
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Get the description of this option
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Wheter this option is essential for the execution of an Application Comand
        /// </summary>
        /// <remarks>
        /// <see langword="false"/> by default for types <see cref="ApplicationCommandOptionType.SubCommand"/> and <see cref="ApplicationCommandOptionType.SubCommandGroup"/>
        /// </remarks>
        bool IsRequired { get; }
        IEnumerable<KeyValuePair<string, object>> Choices { get; }
        IEnumerable<IApplicationCommandOption> Options { get; }
    }
}
