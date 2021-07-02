using Discord.SlashCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.SlashCommands
{
    public class ExecuteResult : IResult
    {
        public SlashCommandError? Error { get; }

        public string ErrorReason { get; }

        public bool IsSuccess { get;}

        public ExecuteResult(SlashCommandError? error, string reason, bool isSuccess)
        {
            Error = error;
            ErrorReason = reason;
            IsSuccess = isSuccess;
        }
    }
}
