using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using top.riverelder.RiverCommand.Utils;

namespace top.riverelder.RiverCommand {
    public abstract class CommandNode<TEnv> {
        public string ParamName { get; set; } = null;
        public bool Spread { get; set; } = false;
        public IList<CommandNode<TEnv>> Children { get; } = new List<CommandNode<TEnv>>();
        public CmdExecutor<TEnv> Executor { get; set; } = null;
        public PreProcess<TEnv> Process { get; set; } = null;

        public CommandNode(string paramName) {
            ParamName = paramName;
        }

        public CommandNode() {
            ParamName = null;
        }

        /// <summary>
        /// 调度参数解析
        /// </summary>
        /// <param name="raw">原始字符串</param>
        /// <param name="env">环境</param>
        /// <param name="args">参数</param>
        /// <param name="reply">回复消息</param>
        /// <returns>是否符合该节点</returns>
        public DispatchResult Dispatch(string raw, TEnv env, Args args, out string reply) {
            if (args == null) {
                args = new Args();
            }
            string err = "参数匹配错误";
            raw = Regex.Replace(raw, @"^\s+", "");
            string next = raw;

            // 匹配参数
            object arg = null;
            if (Spread) { // 收集所有剩下的参数
                List<object> list = new List<object>();
                while (!string.IsNullOrEmpty(next) && Parse(raw, out arg, out int length, out err)) {
                    list.Add(arg);
                    next = Regex.Replace(next.Substring(length), @"^\s+", "");
                }
                if (list.Count == 0) {
                    reply = err;
                    return DispatchResult.Unmatched;
                }
                arg = list.ToArray();
            } else { // 仅检测一个参数
                if (!Parse(raw, out arg, out int length, out err)) {
                    reply = err;
                    return DispatchResult.Unmatched;
                }
                next = Regex.Replace(next.Substring(length), @"^\s+", "");
            }

            // 预处理得到的参数，包括参数的可行检测，以及转换
            if (Process != null && !Process(env, args, out arg)) {
                reply = err;
                return DispatchResult.Unmatched;
            }

            // 如果有参数名，则将该值赋予参数
            if (!string.IsNullOrEmpty(ParamName)) {
                args[ParamName] = arg;
            }

            List<string> tips = new List<string>();
            // 优先匹配子节点，执行子节点的逻辑
            DispatchResult childResult = DispatchResult.MatchedAll;
            foreach (CommandNode<TEnv> child in Children) {
                childResult = child.Dispatch(next, env, args.Derives(), out reply);
                if (childResult == DispatchResult.MatchedAll) {
                    // 如果某个子节点全部匹配则完美收官，返回结果
                    return childResult;
                } else if (childResult == DispatchResult.MatchedSelf) {
                    // 如果某个子节点只匹配了自身，则结束遍历，但是暂不返回结果
                    err = reply;
                    break;
                } else {
                    // 如果某个子节点也根本不匹配，则继续遍历下一个节点
                    tips.Add(child.Tip + (string.IsNullOrEmpty(child.ParamName) ? "" : $"（{child.ParamName}）"));
                }
            }
            // 执行到这里，说明子节点没有一个是完美匹配的
            // 如果自身节点本来有执行逻辑的话，则忽略后续参数，执行自身节点的执行逻辑，并判定为参数匹配成功
            if (Executor != null) {
                reply = Executor(env, args);
                return DispatchResult.MatchedAll;
            } else if (childResult == DispatchResult.MatchedSelf) {
                reply = err;
                return childResult;
            }
            // 到这里说明自己没有执行逻辑，而子节点也一个都没有匹配成功，则视为只匹配了自身
            reply = new StringBuilder()
                .AppendLine("期待：" + string.Join("，", tips))
                .Append("得到：" + (string.IsNullOrEmpty(next) ? "<空>" : next))
                .ToString();
            return DispatchResult.MatchedSelf;
        }

        public CommandNode<TEnv> Then(CommandNode<TEnv> node) {
            Children.Add(node);
            return this;
        }

        public CommandNode<TEnv> Rest(CommandNode<TEnv> node) {
            Children.Add(node.Spreads());
            return this;
        }

        public CommandNode<TEnv> Executes(CmdExecutor<TEnv> executor) {
            Executor = executor;
            return this;
        }

        public CommandNode<TEnv> Handles(PreProcess<TEnv> process) {
            Process = process;
            return this;
        }

        public CommandNode<TEnv> Spreads() {
            Spread = true;
            return this;
        }


        /// <summary>
        /// 该参数的类型提示信息
        /// </summary>
        public abstract string Tip { get; }

        /// <summary>
        /// 解析自身节点的参数
        /// </summary>
        /// <param name="raw">原始字符串</param>
        /// <param name="result">解析结果</param>
        /// <param name="length">街区的字符串长度</param>
        /// <param name="err">错误信息</param>
        /// <returns>是否解析成功</returns>
        public abstract bool Parse(string raw, out object result, out int length, out string err);
    }
}
