using Microsoft.AspNetCore.Components;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Engine.Nodes.Connector.OSC.VRChat;

namespace dOSCEngine.Engine.Blocks.Connectors.OSC.VRChat
{
    public partial class OSCVRCButtonBlock

    {
        [Parameter] public OSCVRCButtonNode Node { get; set; } = null;
    }
}
