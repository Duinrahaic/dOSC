using Blazor.Diagrams.Core.Models;
using dOSCEngine.Engine.Nodes.Connector.VRChat;
using dOSCEngine.Services.Connectors.OSC;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Connectors.VRChat
{
    public partial class OSCVRCChatboxBlock
    {
        [Parameter] public OSCVRCChatboxNode Node { get; set; } = null;
    }
}
