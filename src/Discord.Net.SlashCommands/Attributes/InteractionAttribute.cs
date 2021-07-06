using System;

namespace Discord.SlashCommands
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class InteractionAttribute : Attribute
    {
        public string CustomId { get; }

        public InteractionAttribute (string customId)
        {
            CustomId = customId;
        }
    }
}
