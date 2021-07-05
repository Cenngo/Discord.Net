using Discord.API;
using Discord.API.Rest;
using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Discord.SlashCommands
{
    internal static class SlashCommandRestUtil
    {
        public static API.ApplicationCommandOption ParseApplicationCommandOption (this SlashParameterInfo parameterInfo)
        {
            var option = new API.ApplicationCommandOption
            {
                Name = parameterInfo.Name.ToLower(),
                Description = parameterInfo.Description.ToLower(),
                Required = parameterInfo.IsRequired,
                Type = parameterInfo.DiscordOptionType,
                Choices = parameterInfo.Choices?.Select(x => new ApplicationCommandOptionChoice
                {
                    Name = x.Name.ToLower(),
                    Value = x.Value
                }).ToArray(),
                Options = null
            };

            if (!option.Choices.IsSpecified)
                option.Choices = null;

            return option;
        }

        public static bool TryParseApplicationCommandParams (this SlashCommandInfo commandInfo, out CreateApplicationCommandParams commandParams)
        {
            if (commandInfo.Module != null)
            {
                commandParams = null;
                return false;
            }

            commandParams = new CreateApplicationCommandParams(commandInfo.Name, commandInfo.Description)
            {
                DefaultPermission = commandInfo.DefaultPermission,
                Options = commandInfo.Parameters?.Select(x => x.ParseApplicationCommandOption()).ToArray()
            };

            if (!commandParams.Options.IsSpecified)
                commandParams.Options = null;

            return true;
        }

        public static bool TryParseApplicationCommandOption (this SlashCommandInfo commandInfo, out API.ApplicationCommandOption commandOption)
        {
            if (commandInfo.Module == null)
            {
                commandOption = null;
                return false;
            }

            commandOption = commandInfo.ParseApplicationCommandOption();
            return true;
        }

        public static API.ApplicationCommandOption ParseApplicationCommandOption (this SlashCommandInfo commandInfo)
        {
            var option = new API.ApplicationCommandOption
            {
                Name = commandInfo.Name.ToLower(),
                Description = commandInfo.Description.ToLower(),
                Type = ApplicationCommandOptionType.SubCommand,
                Options = commandInfo.Parameters.Select(x => x.ParseApplicationCommandOption()).ToArray(),
                Choices = null,
                Required = false
            };

            if (!option.Options.IsSpecified)
                option.Options = null;

            return option;
        }
        public static IEnumerable<API.ApplicationCommandOption> GroupParseApplicationCommandOption (this IEnumerable<SlashCommandInfo> commands)
        {
            var standalones = commands.Where(x => x.Group == null);
            var subCommands = commands.Where(x => x.Group != null);

            var result = new List<API.ApplicationCommandOption>();


            foreach (var standalone in standalones)
                result.Add(standalone.ParseApplicationCommandOption());

            var grouped = subCommands.GroupBy(x => x.Group.Name);

            foreach (var group in grouped)
            {
                var description = group.First(x => !string.IsNullOrEmpty(x.Group.Description)).Description;

                var current = new API.ApplicationCommandOption()
                {
                    Name = group.Key.ToLower(),
                    Description = description.ToLower(),
                    Type = ApplicationCommandOptionType.SubCommandGroup,
                    Options = group.Select(x => x.ParseApplicationCommandOption()).ToArray(),
                    Choices = null,
                    Required = false
                };

                if (!current.Options.IsSpecified)
                    current.Options = null;

                result.Add(current);
            }

            return result;
        }

        public static IEnumerable<CreateApplicationCommandParams> GroupParseApplicationCommandParams (this IEnumerable<SlashCommandInfo> commands)
        {
            var standalones = commands.Where(x => string.IsNullOrEmpty(x.Group?.Name));
            var subCommands = commands.Where(x => !string.IsNullOrEmpty(x.Group?.Name));

            var result = new List<CreateApplicationCommandParams>();

            foreach (var standalone in standalones)
                if (standalone.TryParseApplicationCommandParams(out var commandParams))
                    result.Add(commandParams);

            var grouped = commands.GroupBy(x => x.Group.Name);

            foreach (var group in grouped)
            {
                var description = group.First(x => !string.IsNullOrEmpty(x.Group.Description)).Description;
                var options = group.Select(x => x.ParseApplicationCommandOption()).ToArray();

                var module = new CreateApplicationCommandParams(group.Key, description)
                {
                    Options = options,
                    DefaultPermission = true
                };

                if (!module.Options.IsSpecified)
                    module.Options = null;

                result.Add(module);
            }
            return result;
        }

        public static bool TryParseApplicationCommandParams (this SlashModuleInfo moduleInfo, out CreateApplicationCommandParams commandParams)
        {
            if (moduleInfo.IsSubModule)
            {
                commandParams = null;
                return false;
            }

            var options = new List<API.ApplicationCommandOption>();

            options.AddRange(moduleInfo.Commands.GroupParseApplicationCommandOption().ToArray());

            commandParams = new CreateApplicationCommandParams(moduleInfo.Name, moduleInfo.Description)
            {
                DefaultPermission = moduleInfo.DefaultPermission,
                Options = options.ToArray()
            };
            return true;
        }

        public static async Task CreateApplicationCommand (ulong applicationId, BaseDiscordClient discord, SlashModuleInfo module, RequestOptions options)
        {
            var result = new List<CreateApplicationCommandParams>();

            if (string.IsNullOrEmpty(module.Name))
                result.AddRange(module.Commands.GroupParseApplicationCommandParams());
            else if (module.TryParseApplicationCommandParams(out var commandParams))
                result.Add(commandParams);

            foreach (var args in result)
                await discord.ApiClient.CreateGlobalApplicationCommand(applicationId, args);
        }
    }
}
