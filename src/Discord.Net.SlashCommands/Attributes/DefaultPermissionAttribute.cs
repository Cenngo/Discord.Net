using System;

namespace Discord.SlashCommands
{
    /// <summary>
    /// Attribute used to set the "Default Permission" property of an Application Command
    /// </summary>
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
