using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.riverelder.RiverCommand.Nodes {
    public class LiteralCommandNode<TEnv> : CommandNode<TEnv> {
        public string Value { get; }

        public override string Tip => Value;

        public override string Help => Value;
        
        public LiteralCommandNode(string value) {
            Value = value;
        }

        public override bool Parse(string raw, out object result, out int length, out string err) {
            if (raw.StartsWith(Value)) {
                result = Value;
                length = Value.Length;
                err = null;
                return true;
            }
            result = null;
            length = 0;
            err = "参数必须是：" + Value;
            return false;
        }
    }
}
