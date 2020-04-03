using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.riverelder.RiverCommand {
    public delegate string CmdExecutor<TEnv>(TEnv env, Args args);
    public delegate bool PreProcess<TEnv>(TEnv env, Args args, out object arg);
}
