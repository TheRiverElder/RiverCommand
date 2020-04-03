using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace top.riverelder.RiverCommand.Nodes {

    public class AnyCommandNode<TEnv> : CommandNode<TEnv> {

        public static readonly Regex Reg = new Regex("^(“([^“”]+)”|\"((\\\"|[^\"])+)\"|\\S+)");



        public AnyCommandNode(string name) : base(name) { }

        public override string Tip => "字符串";

        public override bool Parse(string raw, out object result, out int length, out string err) {
            Match m = Reg.Match(raw);
            if (!m.Success) {
                result = null;
                length = 0;
                err = "未找到可行字符串";
                return false;
            }


            if (m.Groups[2].Success) {
                result = m.Groups[2].Value.Replace("\\\"", "\"");
            } else if (m.Groups[3].Success) {
                result = m.Groups[3].Value;
            } else {
                result = m.Value;
            }

            length = m.Value.Length;
            err = null;
            return true;
        }
    }
}
