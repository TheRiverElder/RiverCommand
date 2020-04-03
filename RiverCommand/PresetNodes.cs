using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.riverelder.RiverCommand.Nodes;
using top.riverelder.RiverCommand.Utils;

namespace top.riverelder.RiverCommand
{
    public static class PresetNodes
    {
        public static LiteralCommandNode<TEnv> Literal<TEnv>(string word) {
            return new LiteralCommandNode<TEnv>(word);
        }

        public static IntCommandNode<TEnv> Int<TEnv>(string name) {
            return new IntCommandNode<TEnv>(name, NumberType.Int);
        }

        public static IntCommandNode<TEnv> Int<TEnv>(string name, NumberType type) {
            return new IntCommandNode<TEnv>(name, type);
        }

        public static AnyCommandNode<TEnv> Any<TEnv>(string name) {
            return new AnyCommandNode<TEnv>(name);
        }

        public static RestCommandNode<TEnv> Rest<TEnv>(string name) {
            return new RestCommandNode<TEnv>(name);
        }
    }
}
