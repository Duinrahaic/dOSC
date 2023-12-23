using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Connectors.OSC
{
    public partial class OSCBooleanReadBlock
    {
        [Parameter]
        public OSCBooleanReadNode Node { get; set; } = null;
    }
}
