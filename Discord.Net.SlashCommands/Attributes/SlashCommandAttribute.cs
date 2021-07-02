using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.SlashCommands
{
    [AttributeUsage( AttributeTargets.Method, AllowMultiple = false)]
    public class SlashCommandAttribute : Attribute
    {
        public string Name { get; }
        public string Description { get; set; }

        public SlashCommandAttribute ( string name, string description )
        {
            Name = name;
            Description = description;
        }
    }
}
