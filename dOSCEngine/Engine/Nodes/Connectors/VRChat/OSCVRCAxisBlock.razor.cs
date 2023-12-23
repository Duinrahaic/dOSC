using Microsoft.AspNetCore.Components;
using dOSCEngine.Services.Connectors.OSC;

namespace dOSCEngine.Engine.Nodes.Connectors.VRChat
{
    public partial class OSCVRCAxisBlock

    {
        [Parameter] public OSCVRCAxisNode Node { get; set; } = null;
    }
}
