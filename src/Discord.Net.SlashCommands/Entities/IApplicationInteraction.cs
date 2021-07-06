using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.SlashCommands
{
    public interface IApplicationInteraction : ISnowflakeEntity, IDeletable
    {
        ulong ApplicationId { get; }
        InteractionType Type { get; }
        IUser User { get; set; } 
    }
}
