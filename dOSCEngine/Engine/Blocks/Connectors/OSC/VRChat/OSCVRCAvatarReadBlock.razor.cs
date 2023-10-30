using Blazor.Diagrams.Core.Models;
using dOSCEngine.Engine.Nodes.Connector.OSC.VRChat;
using dOSCEngine.Services.Connectors.OSC;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Connectors.OSC.VRChat
{
    public partial class OSCVRCAvatarReadBlock
    {
        [Parameter] public OSCVRCAvatarReadNode Node { get; set; } = null;
    }
}
