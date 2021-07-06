using Discord.SlashCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.SlashCommands
{
    public interface IResult
    {
        SlashCommandError? Error { get; }
        string ErrorReason { get; }
        bool IsSuccess { get; }
    }
}
