using System;

namespace Discord.SlashCommands
{
    /// <summary>
    /// Use to create nested Application Commands. It is sufficient to only populate
    /// <see cref="SlashGroupAttribute.Description"/> in one of the <see cref="SlashGroupAttribute"/> for a group with more than one command.
    /// </summary>
    /// <remarks>
    /// Can be either used to mark a class to tag all of the declared methods or to mark commands individually
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class SlashGroupAttribute : Attribute
    {
        public string Name { get; }
        public string Description { get; }

        public SlashGroupAttribute (string name, string description)
        {
            Name = name;
            Description = description;
        }

        public SlashGroupAttribute(string name)
        {
            Name = name;
        }
    }
}
