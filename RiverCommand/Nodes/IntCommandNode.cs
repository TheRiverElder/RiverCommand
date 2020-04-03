using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using top.riverelder.RiverCommand.Utils;

namespace top.riverelder.RiverCommand.Nodes {
    public class IntCommandNode<TEnv> : CommandNode<TEnv> {
        
        public NumberType Type;

        public IntCommandNode(string name, NumberType type) : base (name) {
            Type = type;
        }

        public override string Tip => Type.Str;

        public override bool Parse(string raw, out object result, out int length, out string err) {
            Match m = Type.Reg.Match(raw);
            if (!m.Success) {
                result = 0;
                length = 0;
                err = "参数不是" + Type.Str;
                return false;
            }

            string s = m.Value;
            if (int.TryParse(s, out int rst)) {
                result = rst;
                length = s.Length;
                err = null;
                return true;
            }
            result = 0;
            length = 0;
            err = "整数解析错误";
            return false;
        }
    }
}
