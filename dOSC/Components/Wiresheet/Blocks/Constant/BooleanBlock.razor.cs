﻿using dOSC.Engine.Nodes.Constant;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Constant
{
    public partial class BooleanBlock
    {
        [Parameter] public BooleanNode Node { get; set; } = null;

    }
}