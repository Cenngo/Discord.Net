using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.SlashCommands
{
    /// <summary>
    /// Use to change the default description of an Application Command element
    /// </summary>
    public class DescriptionAttribute : Attribute
    {
        public string Name { get; set; } = null;
        public string Description { get; } = null;

        public DescriptionAttribute(string name, string description = null)
        {
            Name = name;
            Description = description;
        }
    }
}
