using dOSCEngine.Engine.Nodes.Constant;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Constant
{
    public partial class LogicBlock
    {
        [Parameter] public LogicNode Node { get; set; } = null;

    }
}
