using System;
using System.Threading.Tasks;

namespace Discord.SlashCommands
{
    public interface ITypeReader
    {
        Task<TypeReaderResult> ReadAsync (ISlashCommandContext context, object input, IServiceProvider services);
    }
}
