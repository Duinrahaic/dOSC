using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Connectors.OSC
{
    public partial class OSCBooleanBlock
    {
        [Parameter]
        public OSCBooleanNode Node { get; set; } = null;
    }
}
