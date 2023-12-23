using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dOSCEngine.Engine.Nodes.Connectors.VRChat
{
    public partial class OSCVRCAvatarBooleanReadBlock
    {
        [Parameter] public OSCVRCAvatarBooleanReadNode Node { get; set; }
    }
}
