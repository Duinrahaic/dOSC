using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Math
{
    public partial class SquareRootBlock
    {
        [Parameter] public SquareRootNode Node { get; set; } = null;
    }
}
