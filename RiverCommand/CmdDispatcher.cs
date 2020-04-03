using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using top.riverelder.RiverCommand.Nodes;
using top.riverelder.RiverCommand.Utils;

namespace top.riverelder.RiverCommand {

    public class CmdDispatcher<TEnv> {

        private Dictionary<string, string> aliases = new Dictionary<string, string>();
        private Dictionary<string, LiteralCommandNode<TEnv>> commands = new Dictionary<string, LiteralCommandNode<TEnv>>();

        public ICollection<LiteralCommandNode<TEnv>> Commands => commands.Values;

        public LiteralCommandNode<TEnv> this[string head] {
            get {
                if (commands.TryGetValue(head, out LiteralCommandNode<TEnv> cmd)) {
                    return cmd;
                }
                return null;
            }
        }

        public void Register(ICmdEntry<TEnv> cmd) {
            cmd.OnRegister(this);
        }

        public void Register(LiteralCommandNode<TEnv> root) {
            commands[root.Value] = root;
        }

        public void SetAlias(string alias, string replacement) {
            aliases[alias] = replacement;
        }

        public bool Dispatch(string raw, TEnv env, out string reply) {
            raw = Regex.Replace(raw, @"^\s+", "");
            Match m = Regex.Match(raw, @"\s+");
            int index = m.Success ? m.Index : raw.Length;
            string alias = raw.Substring(0, index);
            if (aliases.TryGetValue(alias, out string act)) {
                raw = act + raw.Substring(index);
            }
            m = Regex.Match(raw, @"\s+");
            index = m.Success ? m.Index : raw.Length;
            string head = raw.Substring(0, index);
            //string body = raw.Substring(index + (m.Success ? m.Value.Length : 0));
            if (commands.TryGetValue(head, out LiteralCommandNode<TEnv> node)) {
                return node.Dispatch(raw, env, new Args(), out reply) == DispatchResult.MatchedAll;
            } else {
                reply = "未知指令：" + head;
                return false;
            }
        }
    }
}
