using System;

namespace TestApp
{
    class Program
    {
        static void Main ( string[] args )
        {
            var bot = new Bot();
            bot.RunAsync().GetAwaiter().GetResult();
        }
    }
}
