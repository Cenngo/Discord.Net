using System;

namespace Discord.SlashCommands
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAttribute : Attribute
    {
        public ulong Id { get; }
        public ApplicationCommandPermissionType Type { get; } = ApplicationCommandPermissionType.User;
        public bool Allowed = false;

        public PermissionAttribute (ulong id)
        {
            Id = id;
        }
    }
}
