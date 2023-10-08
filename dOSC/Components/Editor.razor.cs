using Blazor.Diagrams;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using dOSC.Components.Wiresheet.Blocks.Connectors.OSC.Button;
using dOSC.Components.Wiresheet.Blocks.Logic;
using dOSC.Components.Wiresheet.Blocks.Math;
using dOSC.Engine.Nodes;
using dOSC.Engine.Nodes.Connector.OSC;
using dOSC.Engine.Nodes.Logic;
using dOSC.Engine.Nodes.Math;
using dOSC.Services;
using dOSC.Services.Connectors.OSC;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System;

namespace dOSC.Components
{
    public partial class Editor
    {
        [Inject]
        private OSCService? _OSC { get; set; }
        [Inject]
        private dOSCEngine? _Engine { get; set; }

        [Parameter]
        public dOSCWiresheet? Wiresheet { get; set; } = new();


        private BlazorDiagram BD = new(dOSCWiresheetConfiguration.Options);


        protected override void OnParametersSet()
        {
            
        }

      
        public void Reload()
        {
            this.StateHasChanged();
        }




      
    }
}

	


