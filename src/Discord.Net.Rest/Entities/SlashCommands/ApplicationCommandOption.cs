using System.Collections.Generic;
using System.Linq;
using Model = Discord.API.ApplicationCommandOption;

namespace Discord.Rest
{
    public class ApplicationCommandOption : IApplicationCommandOption
    {
        private const int MaxOptionDepth = 3;

        public ApplicationCommandOptionType OptionType { get; }

        public string Name { get; }

        public string Description { get; }

        public bool IsRequired { get; }
        public IReadOnlyDictionary<string, object> Choices { get; }

        public IReadOnlyList<ApplicationCommandOption> Options { get; }

        IEnumerable<KeyValuePair<string, object>> IApplicationCommandOption.Choices => Choices;

        IEnumerable<IApplicationCommandOption> IApplicationCommandOption.Options => Options;

        internal ApplicationCommandOption ( Model model )
        {
            Name = model.Name;
            Description = model.Description;
            OptionType = model.Type;
            if (model.Required.IsSpecified)
                IsRequired = model.Required.Value;
            if (model.Choices.IsSpecified)
                Choices = model.Choices.Value.ToDictionary(x => x.Name, x => x.Value);
            if (model.Options.IsSpecified)
                Options = model.Options.Value.Select(x => ApplicationCommandOption.Create(x, MaxOptionDepth)).ToList();
        }

        internal static ApplicationCommandOption Create(Model model, int ttl)
        {
            if (ttl <= 0)
                throw new System.Exception("Recursive model creation is dumped, children count is higher than expected");
            else
                return Create(model, --ttl);
        }
    }
}
