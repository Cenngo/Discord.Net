using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    public static class SlashCommandUtility
    {
        public static ApplicationCommandOptionType GetDiscordOptionType ( Type type )
        {
            if (type == typeof(string))
                return ApplicationCommandOptionType.String;
            else if (type == typeof(int))
                return ApplicationCommandOptionType.Integer;
            else if (type == typeof(bool))
                return ApplicationCommandOptionType.Boolean;
            else if (type.IsAssignableFrom(typeof(IMentionable)))
                return ApplicationCommandOptionType.Mentionable;
            else if (type.IsAssignableFrom(typeof(IRole)))
                return ApplicationCommandOptionType.Role;
            else if (type.IsAssignableFrom(typeof(IChannel)))
                return ApplicationCommandOptionType.Channel;
            else if (type.IsAssignableFrom(typeof(IUser)))
                return ApplicationCommandOptionType.User;
            else
                throw new ArgumentException("Type of parameter used is not supported by Discord");
        }

        public static ApplicationCommandOptionType GetDiscordOptionType<T> ( ) =>
            GetDiscordOptionType(typeof(T));

        public static Type GetParameterType(ApplicationCommandOptionType type)
        {
            return type switch
            {
                ApplicationCommandOptionType.String => typeof(string),
                ApplicationCommandOptionType.Integer => typeof(int),
                ApplicationCommandOptionType.Boolean => typeof(bool),
                ApplicationCommandOptionType.Mentionable => typeof(IMentionable),
                ApplicationCommandOptionType.Role => typeof(IRole),
                ApplicationCommandOptionType.Channel => typeof(IChannel),
                ApplicationCommandOptionType.User => typeof(IUser)
            };
        }
    }
}
