using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Math
{
    public partial class RollingAverageBlock
    {
        [Parameter] public RollingAverageNode Node { get; set; } = null;

    }
}