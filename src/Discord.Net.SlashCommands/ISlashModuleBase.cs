using Discord.SlashCommands.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.SlashCommands
{
    internal interface ISlashModuleBase
    {
        void SetContext (ISlashCommandContext context);

        void BeforeExecute (SlashCommandInfo command);

        void AfterExecute (SlashCommandInfo command);

        void OnModuleBuilding (SlashCommandService commandService, SlashModuleBuilder builder);
    }
}
