using System.Collections.Generic;

namespace Discord
{
    public interface IApplicationCommand : ISnowflakeEntity
    {
        string Name { get; }
        string Description { get; }
        ulong ApplicationId { get; }
        bool DefaultPermission { get; }
        IGuild? Guild { get; }
        IEnumerable<IApplicationCommandOption> Options { get; }
    }
}
