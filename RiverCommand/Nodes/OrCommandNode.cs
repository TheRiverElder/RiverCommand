using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.riverelder.RiverCommand.Nodes {
    public class OrCommandNode<TEnv> : CommandNode<TEnv> {

        public string[] ValidValues;

        public OrCommandNode(string name, string[] validValues) : base(name) {
            ValidValues = validValues;
            Tip = string.Join("，", ValidValues);
        }

        public override string Tip { get; }

        public override bool Parse(string raw, out object result, out int length, out string err) {
            foreach (string id in ValidValues) {
                if (raw.StartsWith(id)) {
                    result = id;
                    length = id.Length;
                    err = null;
                    return true;
                }
            }
            result = 0;
            length = 0;
            err = "参数必须是以下之一：" + Tip;
            return false;
        }
    }
}
