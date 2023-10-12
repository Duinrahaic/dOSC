using Blazor.Diagrams.Core.Models;
using dOSC.Engine.Nodes.Math;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Math
{
    public partial class DivisionBlock
    {
        [Parameter] public DivisionNode Node { get; set; } = null;
    }
}
