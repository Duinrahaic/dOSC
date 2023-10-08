using dOSC.Engine.Nodes.Logic;
using Microsoft.AspNetCore.Components;
using E = dOSC.Engine.Nodes.Logic;

namespace dOSC.Components.Wiresheet.Blocks.Logic
{
    public partial class AndBlock
    {
        [Parameter] public AndNode Node {get;set;} = null;

    }
}
