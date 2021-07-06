using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.SlashCommands
{
    public class SlashCommandServiceConfig
    {
        public bool RunAsync { get; set; } = true;
        public bool ThrowOnError { get; set; } = false;
    }
}
