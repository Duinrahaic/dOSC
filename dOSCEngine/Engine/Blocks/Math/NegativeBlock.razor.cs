using Blazor.Diagrams.Core.Models;
using dOSCEngine.Engine.Nodes.Math;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Math
{
    public partial class NegativeBlock
    {
        [Parameter] public NegativeNode Node { get; set; } = null;
    }
}
