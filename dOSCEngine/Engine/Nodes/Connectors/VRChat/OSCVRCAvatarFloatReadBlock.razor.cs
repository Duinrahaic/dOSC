using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dOSCEngine.Engine.Nodes.Connectors.VRChat
{
    public partial class OSCVRCAvatarFloatReadBlock
    {
        [Parameter] public OSCVRCAvatarFloatReadNode Node { get; set; }
    }
}
