using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.riverelder.RiverCommand.Nodes {
    public class RestCommandNode<TEnv> : CommandNode<TEnv> {


        public RestCommandNode(string name) : base(name) {}

        public override string Tip => "非空字符串";

        public override bool Parse(string raw, out object result, out int length, out string err) {
            result = raw;
            if (string.IsNullOrEmpty(raw)) {
                length = 0;
                err = "剩余字符串为空";
                return false;
            } else {
                length = raw.Length;
                err = null;
                return true;
            }
        }
    }
}
