using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.riverelder.RiverCommand.ParamParsers;
using top.riverelder.RiverCommand.Utils;

namespace top.riverelder.RiverCommand {
    public static class PresetNodes {

        public static CommandNode<TEnv> Literal<TEnv>(string word) {
            return new CommandNode<TEnv>(new LiteralParser(word));
        }

        public static CommandNode<TEnv> Int<TEnv>(string name) {
            return new CommandNode<TEnv>(name, new IntParser());
        }
        
        public static CommandNode<TEnv> String<TEnv>(string name) {
            return new CommandNode<TEnv>(name, new StringParser());
        }
    }
}
