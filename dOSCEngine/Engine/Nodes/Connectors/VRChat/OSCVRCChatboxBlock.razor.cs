using Blazor.Diagrams.Core.Models;
using dOSCEngine.Services.Connectors.OSC;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Connectors.VRChat
{
    public partial class OSCVRCChatboxBlock
    {
        [Parameter] public OSCVRCChatboxNode Node { get; set; } = null;
    }
}
