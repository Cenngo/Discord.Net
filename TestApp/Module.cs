using Discord;
using Discord.SlashCommands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    [SlashGroup("rainbow6", "test des")]
    public class Module : CommandBase<SlashCommandContext>
    {
        public Module ( )
        {
            Console.WriteLine("module init");
        }
        [SlashGroup("casual", "test des")]
        [SlashCommand("get", "get casual stats")]
        public async Task GetCasual ( string user )
        {
            Console.WriteLine("works");
        }

        [SlashCommand("ranked", "get ranked stats")]
        public async Task GetRanked(string user)
        {
            Console.WriteLine("works");
        }
    }
}
