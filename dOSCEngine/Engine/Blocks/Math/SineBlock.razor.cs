﻿using Blazor.Diagrams.Core.Models;
using dOSCEngine.Engine.Nodes.Math;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Math
{
    public partial class SineBlock
    {
        [Parameter] public SineNode Node { get; set; } = null;
    }
}
