using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Math
{
    public partial class SineBlock
    {
        [Parameter] public SineNode Node { get; set; } = null;
    }
}
