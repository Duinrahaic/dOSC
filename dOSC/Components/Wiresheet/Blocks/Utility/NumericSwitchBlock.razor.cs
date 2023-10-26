using dOSC.Engine.Nodes.Utility;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Utility
{
    public partial class NumericSwitchBlock
    {
        [Parameter] 
        public NumericSwitchNode Node { get; set; } = null;

    }
}
