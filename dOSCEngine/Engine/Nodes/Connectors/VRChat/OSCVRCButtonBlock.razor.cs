using Microsoft.AspNetCore.Components;
using dOSCEngine.Services.Connectors.OSC;

namespace dOSCEngine.Engine.Nodes.Connectors.VRChat
{
    public partial class OSCVRCButtonBlock

    {
        [Parameter] public OSCVRCButtonNode Node { get; set; } = null;
    }
}
