using System;

namespace Discord.SlashCommands
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class DefaultPermissionAttribute : Attribute
    {
        public bool Allow { get; }

        public DefaultPermissionAttribute (bool allow)
        {
            Allow = allow;
        }
    }
}
