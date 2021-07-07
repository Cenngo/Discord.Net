using System;

namespace Discord
{
    public static class SlashCommandUtility
    {
        public static ApplicationCommandOptionType GetDiscordOptionType (Type type)
        {
            if (type == typeof(string))
                return ApplicationCommandOptionType.String;
            else if (type == typeof(int))
                return ApplicationCommandOptionType.Integer;
            else if (type == typeof(bool))
                return ApplicationCommandOptionType.Boolean;
            else if (typeof(IRole).IsAssignableFrom(type))
                return ApplicationCommandOptionType.Role;
            else if (typeof(IChannel).IsAssignableFrom(type))
                return ApplicationCommandOptionType.Channel;
            else if (typeof(IUser).IsAssignableFrom(type))
                return ApplicationCommandOptionType.User;
            else if (typeof(IMentionable).IsAssignableFrom(type))
                return ApplicationCommandOptionType.Mentionable;
            else
                throw new ArgumentException("Type of parameter supplied is not supported by Discord");
        }

        public static ApplicationCommandOptionType GetDiscordOptionType<T> ( ) =>
            GetDiscordOptionType(typeof(T));

        public static Type GetParameterType (ApplicationCommandOptionType type)
        {
            return type switch
            {
                ApplicationCommandOptionType.String => typeof(string),
                ApplicationCommandOptionType.Integer => typeof(int),
                ApplicationCommandOptionType.Boolean => typeof(bool),
                ApplicationCommandOptionType.Mentionable => typeof(IMentionable),
                ApplicationCommandOptionType.Role => typeof(IRole),
                ApplicationCommandOptionType.Channel => typeof(IChannel),
                ApplicationCommandOptionType.User => typeof(IUser),
                _ => throw new ArgumentException("Provided option type cannot be used as a method parameter.")
            };
        }
    }
}
