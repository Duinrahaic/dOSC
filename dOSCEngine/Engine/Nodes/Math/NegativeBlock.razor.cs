using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Math
{
    public partial class NegativeBlock
    {
        [Parameter] public NegativeNode Node { get; set; } = null;
    }
}
