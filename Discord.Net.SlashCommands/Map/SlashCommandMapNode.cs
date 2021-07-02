using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Discord.SlashCommands
{
    internal class SlashCommandMapNode
    {
        

        private readonly ConcurrentDictionary<string, SlashCommandMapNode> _nodes;
        public SlashCommandMapNode (string test )
        {
            
        }
    }
}
