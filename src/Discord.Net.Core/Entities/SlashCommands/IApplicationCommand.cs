using System.Collections.Generic;

namespace Discord
{
    /// <summary>
    /// Represents an Discord Application Command
    /// </summary>
    public interface IApplicationCommand : ISnowflakeEntity
    {
        /// <summary>
        /// Get the name of this command
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Get the description of this command
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Get the Snowflake ID of the application this command belongs to
        /// </summary>
        ulong ApplicationId { get; }
        /// <summary>
        /// Wheter this command is executable by defult
        /// </summary>
        bool DefaultPermission { get; }
        /// <summary>
        /// Get the guild this command belongs to if it is a Guild Command
        /// </summary>
        IGuild? Guild { get; }
        IEnumerable<IApplicationCommandOption> Options { get; }
    }
}
