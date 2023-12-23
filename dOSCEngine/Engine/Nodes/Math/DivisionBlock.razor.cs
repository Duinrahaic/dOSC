using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Math
{
    public partial class DivisionBlock
    {
        [Parameter] public DivisionNode Node { get; set; } = null;
    }
}
