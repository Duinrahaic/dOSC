using Blazor.Diagrams.Core.Models;
using dOSCEngine.Engine.Nodes.Math;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Math
{
    public partial class RollingAverageBlock
    {
        [Parameter] public RollingAverageNode Node { get; set; } = null;

    }
}