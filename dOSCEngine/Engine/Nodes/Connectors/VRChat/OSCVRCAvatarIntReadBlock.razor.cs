using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dOSCEngine.Engine.Nodes.Connectors.VRChat
{
    public partial class OSCVRCAvatarIntReadBlock
    {
        [Parameter]
        public OSCVRCAvatarIntReadNode Node { get; set; }
    }
}
