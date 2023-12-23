using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Math
{
    public partial class SummationBlock
    {
        [Parameter] public SummationNode Node { get; set; } = null;
    }
}
