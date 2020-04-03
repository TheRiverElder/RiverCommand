using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.riverelder.RiverCommand.Nodes;

namespace top.riverelder.RiverCommand.Test {
    class Cmd_Item : ICmdEntry<TestEnv> {

        public void OnRegister(CmdDispatcher<TestEnv> dispatcher) {
            LiteralCommandNode<TestEnv> Root = PresetNodes.Literal<TestEnv>("物品");
            Root.Then(PresetNodes.Literal<TestEnv>("创造").Then(PresetNodes.Any<TestEnv>("物品名").Executes((env, args) => "你创造了：" + args.Get<string>("物品名"))))
                .Then(PresetNodes.Literal<TestEnv>("销毁").Then(PresetNodes.Any<TestEnv>("物品名").Executes((env, args) => "你销毁了：" + args.Get<string>("物品名"))))
                .Then(PresetNodes.Literal<TestEnv>("编辑").Then(PresetNodes.Any<TestEnv>("物品名").Executes((env, args) => "你编辑了：" + args.Get<string>("物品名"))))
                ;
            dispatcher.Register(Root);
            dispatcher.SetAlias("创造", "物品 创造");
            dispatcher.SetAlias("销毁", "物品 销毁");
        }
    }
}
