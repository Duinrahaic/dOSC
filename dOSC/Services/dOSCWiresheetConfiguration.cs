using Blazor.Diagrams;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using dOSC.Components.Wiresheet.Blocks.Connectors.OSC.Button;
using dOSC.Components.Wiresheet.Blocks.Constant;
using dOSC.Components.Wiresheet.Blocks.Logic;
using dOSC.Components.Wiresheet.Blocks.Math;
using dOSC.Engine.Nodes.Connector.OSC;
using dOSC.Engine.Nodes.Constant;
using dOSC.Engine.Nodes.Logic;
using dOSC.Engine.Nodes.Math;

namespace dOSC.Services
{
    public static class dOSCWiresheetConfiguration
    {
        public static BlazorDiagramOptions Options = new BlazorDiagramOptions
        {
            AllowMultiSelection = true,
            GridSnapToCenter = true,
            
            Zoom =
            {
                Enabled = true,
                Inverse = true,
            },
            AllowPanning = true,
            Links =
            {
                DefaultRouter = new NormalRouter(),
                DefaultPathGenerator = new SmoothPathGenerator()
            },
            Groups =
            {
                Enabled =  true,
            },
            
            
        };

        public static void RegisterBlocks(this BlazorDiagram BD)
        {
            BD.RegisterComponent<AndNode, AndBlock>();
            BD.RegisterComponent<OrNode, OrBlock>();
            BD.RegisterComponent<BooleanNode, BooleanBlock>();
            BD.RegisterComponent<AddNode, AddBlock>();
            BD.RegisterComponent<SubtractNode, SubstractBlock>();
            BD.RegisterComponent<NumericNode, NumericBlock>();
            BD.RegisterComponent<SummationNode, SummationBlock>();
            BD.RegisterComponent<MultiplicationNode, MultiplicationBlock>();
            BD.RegisterComponent<DivisionNode, DivisionBlock>();
            BD.RegisterComponent<SineNode, SineBlock>();
            BD.RegisterComponent<OSCVRCButtonNode, OSCVRCButtonBlock>();
            BD.RegisterComponent<EqualNode, EqualBlock>();
        }
    }
}
