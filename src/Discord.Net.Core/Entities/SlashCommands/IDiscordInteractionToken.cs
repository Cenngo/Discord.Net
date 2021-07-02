using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    public interface IDiscordInteractionToken
    {
        string Token { get; }
        bool IsValid { get; }
    }
}
