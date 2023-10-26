using Blazor.Diagrams;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using dOSC.Components.Wiresheet.Blocks.Connectors.Activity;
using dOSC.Components.Wiresheet.Blocks.Connectors.OSC;
using dOSC.Components.Wiresheet.Blocks.Connectors.OSC.VRChat;
using dOSC.Components.Wiresheet.Blocks.Constant;
using dOSC.Components.Wiresheet.Blocks.Logic;
using dOSC.Components.Wiresheet.Blocks.Math;
using dOSC.Components.Wiresheet.Blocks.Utility;
using dOSC.Engine.Nodes.Connector.Activity;
using dOSC.Engine.Nodes.Connector.OSC;
using dOSC.Engine.Nodes.Connector.OSC.VRChat;
using dOSC.Engine.Nodes.Constant;
using dOSC.Engine.Nodes.Logic;
using dOSC.Engine.Nodes.Math;
using dOSC.Engine.Nodes.Utility;
using System.Diagnostics;

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
            // Activity
            BD.RegisterComponent<PulsoidNode, PulsoidBlock>();

            // OSC
            BD.RegisterComponent<OSCBooleanNode, OSCBooleanBlock>();
            BD.RegisterComponent<OSCBooleanReadNode, OSCBooleanReadBlock>();
            BD.RegisterComponent<OSCIntNode, OSCIntBlock>();
            BD.RegisterComponent<OSCIntReadNode, OSCIntReadBlock>();
            BD.RegisterComponent<OSCFloatNode, OSCFloatBlock>();
            BD.RegisterComponent<OSCFloatReadNode, OSCFloatReadBlock>();

            // OSC - VRChat
            BD.RegisterComponent<OSCVRCButtonNode, OSCVRCButtonBlock>();
            BD.RegisterComponent<OSCVRCAvatarReadNode, OSCVRCAvatarReadBlock>();
            BD.RegisterComponent<OSCVRCChatboxNode, OSCVRCChatboxBlock>();
            BD.RegisterComponent<OSCVRCAxisNode, OSCVRCAxisBlock>();

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
            BD.RegisterComponent<AbsoluteNode, AbsoluteBlock>();
            BD.RegisterComponent<AddNode, AddBlock>();
            BD.RegisterComponent<AverageNode, AverageBlock>();
            BD.RegisterComponent<ClampNode, ClampBlock>();
            BD.RegisterComponent<DivisionNode, DivisionBlock>();
            BD.RegisterComponent<MaxNode, MaxBlock>();
            BD.RegisterComponent<MinNode, MinBlock>();
            BD.RegisterComponent<MultiplicationNode, MultiplicationBlock>();
            BD.RegisterComponent<NegativeNode, NegativeBlock>();
            BD.RegisterComponent<PowerNode, PowerBlock>();
            BD.RegisterComponent<SineNode, SineBlock>();
            BD.RegisterComponent<SquareRootNode, SquareRootBlock>();
            BD.RegisterComponent<SubtractNode, SubstractBlock>();
            BD.RegisterComponent<SummationNode, SummationBlock>();

            // Utility
            BD.RegisterComponent<LogicSwitchNode, LogicSwitchBlock>();
            BD.RegisterComponent<NumericSwitchNode, NumericSwitchBlock>();
        }
    }
}
