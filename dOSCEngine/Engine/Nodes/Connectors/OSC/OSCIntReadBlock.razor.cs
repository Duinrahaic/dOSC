using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Connectors.OSC
{
    public partial class OSCIntReadBlock
    {
        [Parameter]
        public OSCIntReadNode Node { get; set; } = null;
    }
}
