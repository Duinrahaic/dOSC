using dOSC.Engine.Nodes.Connector.OSC;
using Microsoft.AspNetCore.Components;
using dOSC.Services.Connectors.OSC;

namespace dOSC.Components.Wiresheet.Blocks.Connectors.OSC.Button
{
    public partial class OSCVRCAxisBlock

    {
        [Parameter] public OSCVRCAxisNode Node { get; set; } = null;
    }
}
