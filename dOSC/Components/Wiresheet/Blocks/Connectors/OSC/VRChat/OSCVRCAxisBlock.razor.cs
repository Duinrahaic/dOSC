using Microsoft.AspNetCore.Components;
using dOSC.Services.Connectors.OSC;
using dOSC.Engine.Nodes.Connector.OSC.VRChat;

namespace dOSC.Components.Wiresheet.Blocks.Connectors.OSC.VRChat
{
    public partial class OSCVRCAxisBlock

    {
        [Parameter] public OSCVRCAxisNode Node { get; set; } = null;
    }
}
