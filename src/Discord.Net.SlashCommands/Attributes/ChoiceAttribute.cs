using System;

namespace Discord.SlashCommands
{
    /// <summary>
    /// Attribute used to add a pre-determined argument value
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public class ChoiceAttribute : Attribute
    {
        public string Name { get; }
        public SlashCommandChoiceType Type { get; }
        public object Value { get; }

        private ChoiceAttribute (string name)
        {
            Name = name;
        }

        public ChoiceAttribute (string name, string value) : this(name)
        {
            Type = SlashCommandChoiceType.String;
            Value = value;
        }

        public ChoiceAttribute (string name, int value) : this(name)
        {
            Type = SlashCommandChoiceType.Integer;
            Value = value;
        }
    }
}
