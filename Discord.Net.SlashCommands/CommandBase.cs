using Discord.SlashCommands.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.SlashCommands
{
    public abstract class CommandBase<T> : ISlashModuleBase where T : ISlashCommandContext
    {
        public void AfterExecute (SlashCommandInfo command) { }
        public void BeforeExecute (SlashCommandInfo command) { }
        public void OnModuleBuilding (SlashCommandService commandService, SlashModuleBuilder builder) { }
        public void SetContext (ISlashCommandContext context) { }
    }
}
