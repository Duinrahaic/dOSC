using Microsoft.AspNetCore.Components;
using System.Diagnostics.Metrics;

namespace dOSCEngine.Engine.Nodes.Logic
{
    public partial class NotEqualBlock
    {
        [Parameter] public NotEqualNode Node { get; set; } = null;


    }
}
