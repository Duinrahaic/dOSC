using Blazor.Diagrams.Core.Models;
using dOSC.Engine.Nodes.Math;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Math
{
    public partial class SquareRootBlock
    {
        [Parameter] public SquareRootNode Node { get; set; } = null;
    }
}
