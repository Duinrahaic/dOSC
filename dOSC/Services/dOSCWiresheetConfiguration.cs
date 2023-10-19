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
using dOSC.Engine.Nodes.Utility;

namespace dOSC.Services
{
    public static class dOSCWiresheetConfiguration
    {
        public static BlazorDiagramOptions Options = new BlazorDiagramOptions
        {
            GridSnapToCenter = true,
            Zoom =
            {
                Enabled = true,
                Inverse = true,
                Maximum = 3.0
            },
            AllowPanning = true,
            Links =
            {
                DefaultRouter = new NormalRouter(),
                DefaultPathGenerator = new SmoothPathGenerator()
            },
            Groups =
            {
                Enabled =  false,
            },
            Constraints =
            {
                
            }
            
            
        };

        public static void RegisterBlocks(this BlazorDiagram BD)
        {
            // Connectors
            BD.RegisterComponent<OSCVRCButtonNode, OSCVRCButtonBlock>();

            // Constants
            BD.RegisterComponent<LogicNode, LogicBlock>();
            BD.RegisterComponent<NumericNode, NumericBlock>();

            // Logic
            BD.RegisterComponent<AndNode, AndBlock>();
            BD.RegisterComponent<EqualNode, EqualBlock>();
            BD.RegisterComponent<GreaterThanNode, GreaterThanBlock>();
            BD.RegisterComponent<GreaterThanEqualNode, GreaterThanEqualBlock>();
            BD.RegisterComponent<LessThanNode, LessThanBlock>();
            BD.RegisterComponent<LessThanEqualNode, LessThanEqualBlock>();
            BD.RegisterComponent<NotNode, NotBlock>();
            BD.RegisterComponent<NotEqualNode, NotEqualBlock>();
            BD.RegisterComponent<OrNode, OrBlock>();
            BD.RegisterComponent<XOrNode, XOrBlock>();

            // Math
            BD.RegisterComponent<AddNode, AddBlock>();
            BD.RegisterComponent<SubtractNode, SubstractBlock>();
            BD.RegisterComponent<SummationNode, SummationBlock>();
            BD.RegisterComponent<MultiplicationNode, MultiplicationBlock>();
            BD.RegisterComponent<DivisionNode, DivisionBlock>();

            // Utility
            BD.RegisterComponent<SineNode, SineBlock>();
        }
    }
}
