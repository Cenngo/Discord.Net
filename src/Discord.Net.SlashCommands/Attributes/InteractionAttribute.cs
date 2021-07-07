using System;

namespace Discord.SlashCommands
{
    /// <summary>
    /// Attribute for tagging a Message Component interaction handler, CustomId represents
    /// the CustomId of the Message Component that will be handled. This will stack with <see cref="SlashGroupAttribute"/>
    /// </summary>
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
