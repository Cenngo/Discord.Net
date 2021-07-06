using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.SlashCommands
{
    public class SlashGroupInfo
    {
        public string Name { get; }
        public string Description { get; }
        public SlashGroupInfo(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
