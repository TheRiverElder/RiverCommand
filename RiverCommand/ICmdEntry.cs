using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.riverelder.RiverCommand.Nodes;

namespace top.riverelder.RiverCommand {
    public interface ICmdEntry<TEnv> {

        void OnRegister(CmdDispatcher<TEnv> dispatcher);

    }
}
