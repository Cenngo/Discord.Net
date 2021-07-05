using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    [Flags]
    public enum InteractionApplicationCommandCallbackFlags
    {
        Ephemeral = 1<<6
    }
}
