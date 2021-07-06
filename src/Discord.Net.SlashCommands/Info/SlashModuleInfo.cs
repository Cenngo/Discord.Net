using Discord.SlashCommands.Builders;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Discord.SlashCommands
{
    public class SlashModuleInfo
    {
        public SlashCommandService CommandService { get; }
        public string Name { get; }
        public string Description { get; }
        public bool DefaultPermission { get; }
        public IReadOnlyList<SlashModuleInfo> SubModules { get; }
        public IReadOnlyList<SlashCommandInfo> Commands { get; }
        public IReadOnlyCollection<SlashInteractionInfo> Interactions { get; }
        public SlashModuleInfo Parent { get; }
        public IReadOnlyList<Attribute> Attributes { get; }
        public bool IsSubModule => Parent != null;

        internal SlashModuleInfo (SlashModuleBuilder builder, SlashCommandService commandService = null, SlashModuleInfo parent = null)
        {
            Parent = parent;
            CommandService = commandService ?? builder.CommandService;

            Name = builder.Name;
            Description = builder.Description;
            DefaultPermission = builder.DefaultPermission;
            SubModules = BuildSubModules(builder, commandService).ToImmutableArray();
            Commands = BuildCommands(builder).ToImmutableArray();
            Interactions = BuildInteractions(builder).ToImmutableArray();
            Attributes = BuildAttributes(builder).ToImmutableArray();
        }

        private IEnumerable<SlashModuleInfo> BuildSubModules (SlashModuleBuilder builder, SlashCommandService commandService)
        {
            var result = new List<SlashModuleInfo>();

            foreach (var submodule in builder.SubModules)
                result.Add(submodule.Build(this, commandService));

            return result;
        }

        private IEnumerable<SlashCommandInfo> BuildCommands (SlashModuleBuilder builder)
        {
            var result = new List<SlashCommandInfo>();

            foreach (SlashCommandBuilder commandBuilder in builder.Commands)
                result.Add(commandBuilder.Build(this, CommandService));

            return result;
        }

        private IEnumerable<SlashInteractionInfo> BuildInteractions(SlashModuleBuilder builder)
        {
            var result = new List<SlashInteractionInfo>();

            foreach (var interactionBuilder in builder.Interactions)
                result.Add(interactionBuilder.Build(this, CommandService));

            return result;
        }

        private IEnumerable<Attribute> BuildAttributes (SlashModuleBuilder builder)
        {
            var result = new List<Attribute>();
            var currentParent = builder;

            while (currentParent != null)
            {
                result.AddRange(currentParent.Attributes);
                currentParent = currentParent.Parent;
            }

            return result;
        }
    }
}
