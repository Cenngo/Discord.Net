using Discord.SlashCommands.Builders;
using System;
using System.Collections.Generic;

namespace Discord.SlashCommands
{
    public class SlashParameterInfo
    {
        public SlashCommandInfo Command { get; }
        public string Name { get; }
        public string Description { get; }
        public Type ParameterType { get; }
        public bool IsRequired { get; }
        public object DefaultValue { get; }
        public IReadOnlyList<ParameterChoice> Choices { get; }
        public IReadOnlyList<Attribute> Attributes { get; }
        public ApplicationCommandOptionType DiscordOptionType => SlashCommandUtility.GetDiscordOptionType(ParameterType);

        internal SlashParameterInfo (SlashParameterBuilder builder, SlashCommandInfo command)
        {
            Command = command;
            Name = builder.Name;
            Description = builder.Description;
            ParameterType = builder.ParameterType;
            IsRequired = builder.IsRequired;
            DefaultValue = builder.DefaultValue;
            Choices = builder.Choices;
            Attributes = builder.Attributes;
        }

        public override string ToString ( ) => Name;
    }
}
