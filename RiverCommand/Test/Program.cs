using System;

namespace top.riverelder.RiverCommand.Test {
    class Program {
        static TestEnv env = new TestEnv();
        static CmdDispatcher<TestEnv> dispatcher = new CmdDispatcher<TestEnv>();

        static void Main(string[] args) {
            ICmdEntry<TestEnv> cmd = new Cmd_Item();
            dispatcher.Register(cmd);
            

            //Test("物品 创造 UltraSward");
            //Test("物品 销毁 UltraSward");
            //Test("物品 UltraSward");
            //Test("物品 创造");
            //Test("物品 销毁");
            //Test("创造 UltraSward");
            //Test("sth 创造 UltraSward");
            //Test("物品");
            //Test("物品 编辑 UltraSward");
            //Test("物品 编辑");

            string buffer = "";
            while (true) {
                string s = Console.ReadLine();
                if (s.StartsWith(".")) {
                    break;
                }
                if (string.IsNullOrEmpty(s)) {
                    Test(buffer);
                    buffer = "";
                } else {
                    buffer += '\n' + s;
                }
            }
            Console.ReadKey();
        }

        static void Test(string s) {
            Console.WriteLine("运行：" + s);
            Console.Write("结果：");
            if (dispatcher.Dispatch(s, env, out string reply)) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("成功");
            } else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("失败");
            }
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine(reply);
            Console.WriteLine("================================");
        }
    }
}
