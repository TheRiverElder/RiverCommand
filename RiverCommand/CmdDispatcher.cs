﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using top.riverelder.RiverCommand.Utils;

namespace top.riverelder.RiverCommand {

    public class CmdDispatcher<TEnv> {

        private Dictionary<string, string> aliases = new Dictionary<string, string>();
        private Dictionary<string, CommandNode<TEnv>> commands = new Dictionary<string, CommandNode<TEnv>>();

        public ICollection<CommandNode<TEnv>> Commands => commands.Values;

        public CommandNode<TEnv> this[string head] {
            get {
                if (commands.TryGetValue(head, out CommandNode<TEnv> cmd)) {
                    return cmd;
                }
                return null;
            }
        }

        public void Register(ICmdEntry<TEnv> cmd) {
            cmd.OnRegister(this);
        }

        public void Register(string head, params CommandNode<TEnv>[] nodes) {
            CommandNode<TEnv> n = PresetNodes.Literal<TEnv>(head);
            foreach (CommandNode<TEnv> node in nodes) {
                n.AddChild(node);
            }
            commands[head] = n;
        }

        public void SetAlias(string alias, string replacement) {
            aliases[alias] = replacement;
        }

        public bool Dispatch(string raw, TEnv env, out string reply) {
            raw = raw.TrimStart();
            StringReader reader = new StringReader(raw);
            reader.SkipWhiteSpace();
            string alias = reader.ReadToWhiteSpace();
            if (aliases.TryGetValue(alias, out string act)) {
                reader = new StringReader(act + reader.ReadRest());
            }
            reader.Cursor = 0;
            string head = reader.ReadToWhiteSpace();
            reader.Cursor = 0;
            if (commands.TryGetValue(head, out CommandNode<TEnv> node)) {
                return node.Dispatch(reader, env, new Args(), out reply) == DispatchResult.MatchedAll;
            } else {
                reply = "未知指令：" + head;
                return false;
            }
        }
    }
}
