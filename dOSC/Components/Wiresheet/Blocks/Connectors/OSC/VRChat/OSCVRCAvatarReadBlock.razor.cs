using Blazor.Diagrams.Core.Models;
using dOSC.Engine.Nodes.Connector.OSC.VRChat;
using dOSC.Services.Connectors.OSC;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Connectors.OSC.VRChat
{
    public partial class OSCVRCAvatarReadBlock
    {
        [Parameter] public OSCVRCAvatarReadNode Node { get; set; } = null;
    }
}
