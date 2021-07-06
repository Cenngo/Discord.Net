using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.SlashCommands
{
    public class ParameterChoice
    {
        public string Name { get; set; }
        public object Value { get; }

        internal ParameterChoice(string name, string value)
        {
            Name = name;
            Value = value;
        }

        internal ParameterChoice(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}
