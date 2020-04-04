using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.riverelder.RiverCommand.ParamParsers;

namespace top.riverelder.RiverCommand.Test {
    class Cmd_Item : ICmdEntry<TestEnv> {

        public void OnRegister(CmdDispatcher<TestEnv> dispatcher) {
            CommandNode<TestEnv> root = PresetNodes.Literal<TestEnv>("物品");

            DictMapper mapper = new DictMapper()
                .Then("前缀", new StringParser())
                .Then("后缀", new StringParser())
                ;

            Console.WriteLine(string.Join("\n", root.GetHelp()));
            dispatcher.Register("物品",
                PresetNodes.Literal<TestEnv>("创造")
                    .Then(PresetNodes.String<TestEnv>("物品名")
                        .MapDict(mapper)
                        .Executes((env, args, dict) => CreateItem(
                            env.Items, 
                            args.Get<string>("物品名"), 
                            dict.Get<string>("前缀"), 
                            dict.Get<string>("后缀")
                        ))),
                PresetNodes.Literal<TestEnv>("销毁")
                    .Then(PresetNodes.String<TestEnv>("物品名")
                    .Executes((env, args, dict) => DestoryItem(
                        env.Items, 
                        args.Get<string>("物品名")
                    ))),
                PresetNodes.Literal<TestEnv>("编辑")
                    .Then(PresetNodes.String<TestEnv>("物品名")
                        .Then(PresetNodes.String<TestEnv>("新名字")
                        .MapDict(mapper)
                        .Executes((env, args, dict) => EditItem(
                            env.Items, 
                            args.Get<string>("物品名"), 
                            args.Get<string>("新名字"), 
                            dict.Get<string>("前缀"), 
                            dict.Get<string>("后缀")
                        )))),
                PresetNodes.Literal<TestEnv>("显示")
                    .Executes((env, args, dict) => ListItems(env.Items))
            );

            dispatcher.SetAlias("创造", "物品 创造");
            dispatcher.SetAlias("销毁", "物品 销毁");
        }

        public string CreateItem(HashSet<string> items, string item, string prefix, string suffix) {
            item = prefix + item + suffix;
            if (items.Contains(item)) {
                return "你已经拥有了：" + item;
            }
            items.Add(item);
            return "你创造了：" + item;
        }

        public string DestoryItem(HashSet<string> items, string item) {
            if (!items.Contains(item)) {
                return "你还未拥有：" + item;
            }
            items.Remove(item);
            return "你销毁了：" + item;
        }

        public string EditItem(HashSet<string> items, string item, string newName, string prefix, string suffix) {
            if (!items.Contains(item)) {
                return "你还未拥有：" + item;
            }
            newName = prefix + newName + suffix;
            items.Remove(item);
            items.Add(newName);
            return "你的" + item + "被编辑为：" + newName;
        }

        public string ListItems(HashSet<string> items) {
            if (items.Count == 0) {
                return "你没有物品";
            }

            StringBuilder sb = new StringBuilder().Append("你的物品：");
            foreach (string item in items) {
                sb.AppendLine().Append(item);
            }
            return sb.ToString();
        }
    }
}
