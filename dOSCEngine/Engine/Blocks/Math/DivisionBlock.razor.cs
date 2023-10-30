using Blazor.Diagrams.Core.Models;
using dOSCEngine.Engine.Nodes.Math;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Math
{
    public partial class DivisionBlock
    {
        [Parameter] public DivisionNode Node { get; set; } = null;
    }
}
