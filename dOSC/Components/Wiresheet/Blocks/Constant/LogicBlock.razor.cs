using dOSC.Engine.Nodes.Constant;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Constant
{
    public partial class LogicBlock
    {
        [Parameter] public LogicNode Node { get; set; } = null;

    }
}
