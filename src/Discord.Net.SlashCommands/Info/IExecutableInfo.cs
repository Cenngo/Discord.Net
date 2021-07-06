using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.SlashCommands
{
    internal interface IExecutableInfo
    {
        string Name { get; }
        SlashModuleInfo Module { get; }
        SlashGroupInfo Group { get; }
    }
}
