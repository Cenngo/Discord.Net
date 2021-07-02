using System;

namespace Discord.SlashCommands
{
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
    }
}
