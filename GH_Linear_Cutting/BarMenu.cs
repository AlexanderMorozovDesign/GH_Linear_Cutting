using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper;

namespace GH_Linear_Cutting
{
    public class LogoToolBarMenu : Grasshopper.Kernel.GH_AssemblyPriority
    {
        public override Grasshopper.Kernel.GH_LoadingInstruction PriorityLoad()
        {
            Instances.ComponentServer.AddCategorySymbolName("1D Liner Cutting", 'L');
            return Grasshopper.Kernel.GH_LoadingInstruction.Proceed;
        }
    }
}
