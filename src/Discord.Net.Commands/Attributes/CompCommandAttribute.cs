using System;

namespace Discord.Commands
{
    /// <summary>
    ///     Marks the execution information for a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CompCommandAttribute : CommandAttribute
    {
        /// <summary>
        ///     Gets the name of the command.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets or sets the summary of the command.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        ///     Gets or sets the remarks of the command.
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        ///     Gets the allias collection of the command.
        /// </summary>
        public string[] Alliases { get; }

        /// <summary>
        ///     Initializes a new <see cref="CompCommandAttribute" /> attribute with the specified command information.
        /// </summary>
        /// <param name="name">Name of the command.</param>
        /// <param name="ignoreExtraArgs"></param>
        /// <param name="alliases"></param>
        public CompCommandAttribute(string name, bool ignoreExtraArgs = false, params string[] alliases)
            : base(name, ignoreExtraArgs)
        {
            Name = name;
            Alliases = alliases;
        }
    }
}
