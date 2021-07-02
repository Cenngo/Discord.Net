using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.SlashCommands
{
    public class CommandGroup
    {
        public string Name { get; }
        public string Description { get; }

        internal CommandGroup(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
