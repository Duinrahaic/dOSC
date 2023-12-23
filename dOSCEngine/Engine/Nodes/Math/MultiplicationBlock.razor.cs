using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Math
{
    public partial class MultiplicationBlock
    {
        [Parameter] public MultiplicationNode Node { get; set; } = null;
    }
}
