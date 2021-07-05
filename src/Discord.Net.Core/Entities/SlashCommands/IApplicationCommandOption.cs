using System.Collections.Generic;

namespace Discord
{
    public interface IApplicationCommandOption
    {
        ApplicationCommandOptionType OptionType { get; }
        string Name { get; }
        string Description { get; }
        bool IsRequired { get; }
        IEnumerable<KeyValuePair<string, object>> Choices { get; }
        IEnumerable<IApplicationCommandOption> Options { get; }
    }
}
