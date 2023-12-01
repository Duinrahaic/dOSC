using Blazor.Diagrams;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using dOSCEngine.Engine.Blocks.Connectors.Activity;
using dOSCEngine.Engine.Blocks.Connectors.OSC;
using dOSCEngine.Engine.Blocks.Connectors.VRChat;
using dOSCEngine.Engine.Blocks.Constant;
using dOSCEngine.Engine.Blocks.Logic;
using dOSCEngine.Engine.Blocks.Math;
using dOSCEngine.Engine.Blocks.Utility;
using dOSCEngine.Engine.Nodes.Connector.Activity;
using dOSCEngine.Engine.Nodes.Connector.OSC;
using dOSCEngine.Engine.Nodes.Connector.VRChat;
using dOSCEngine.Engine.Nodes.Constant;
using dOSCEngine.Engine.Nodes.Logic;
using dOSCEngine.Engine.Nodes.Math;
using dOSCEngine.Engine.Nodes.Utility;

namespace dOSCEngine.Services
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

            },
            


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
            BD.RegisterComponent<AvatarParameterBooleanNode, AvatarParameterBooleanBlock>();
            BD.RegisterComponent<AvatarParameterBooleanReadNode, AvatarParameterBooleanReadBlock>();
            BD.RegisterComponent<AvatarParameterIntNode, AvatarParameterIntBlock>();
            BD.RegisterComponent<AvatarParameterIntReadNode, AvatarParameterIntReadBlock>();
            BD.RegisterComponent<AvatarParameterFloatNode, AvatarParameterFloatBlock>();
            BD.RegisterComponent<AvatarParameterFloatReadNode, AvatarParameterFloatReadBlock>();
            BD.RegisterComponent<OSCVRCButtonNode, OSCVRCButtonBlock>();
            BD.RegisterComponent<OSCVRCAvatarIntReadNode, OSCVRCAvatarIntReadBlock>();
            BD.RegisterComponent<OSCVRCAvatarFloatReadNode, OSCVRCAvatarFloatReadBlock>();
            BD.RegisterComponent<OSCVRCAvatarBooleanReadNode, OSCVRCAvatarBooleanReadBlock>();
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
            BD.RegisterComponent<RollingAverageNode, RollingAverageBlock>();
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
