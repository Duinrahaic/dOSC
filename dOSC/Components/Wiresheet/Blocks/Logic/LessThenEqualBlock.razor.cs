﻿using dOSC.Engine.Nodes.Logic;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Logic
{
    public partial class LessThenEqualBlock
    {
        [Parameter] public LessThanEqualNode Node { get; set; } = null;

    }
}
