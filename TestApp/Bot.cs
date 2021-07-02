using Discord.SlashCommands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public class Bot
    {
        public DiscordSocketClient DiscordClient;
        public SlashCommandService Slash;
        public IServiceProvider Services;
        public async Task RunAsync ( )
        {
            DiscordClient = new DiscordSocketClient();
            
            await DiscordClient.LoginAsync(Discord.TokenType.Bot, "NjY2NjMyNDE2NTU0OTA5NzA2.Xh2_0Q.Va49BVYMOh_eeaQ4zK-8pS-kmZs");
            await DiscordClient.StartAsync();

            Slash = new SlashCommandService(DiscordClient);
            Services = new ServiceCollection().BuildServiceProvider();

            DiscordClient.InteractionRecieved += Discord_InteractionRecieved;
            DiscordClient.Ready += Discord_Ready;
            DiscordClient.InteractionRecieved += DiscordClient_InteractionRecieved;
            await Task.Delay(-1);
        }

        private async Task DiscordClient_InteractionRecieved (SocketInteraction arg)
        {
            throw new NotImplementedException();
        }

        private async Task Discord_Ready ( )
        {
            await Slash.AddModules(Assembly.GetExecutingAssembly(), Services);
            await Slash.SyncCommands();
        }

        private async Task Discord_InteractionRecieved ( SocketInteraction arg )
        {
            if(arg is SocketCommandInteraction commandInteraction)
            {
                var ctx = new SlashCommandContext(DiscordClient, arg);
                await Slash.ExecuteAsync(ctx, commandInteraction.Command, Services);
            }    
        }
    }
}
