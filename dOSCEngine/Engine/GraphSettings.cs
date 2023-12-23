using Blazor.Diagrams;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Nodes.Connectors.Activity;
using dOSCEngine.Engine.Nodes.Connectors.OSC;
using dOSCEngine.Engine.Nodes.Connectors.VRChat;
using dOSCEngine.Engine.Nodes.Constant;
using dOSCEngine.Engine.Nodes.Logic;
using dOSCEngine.Engine.Nodes.Math;
using dOSCEngine.Engine.Nodes.Utility;
using dOSCEngine.Services;

namespace dOSCEngine.Engine
{
    public static class GraphSettings
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
                DefaultPathGenerator = new SmoothPathGenerator(),
                EnableSnapping = true,
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
            BD.RegisterComponent<DelayNode, DelayBlock>();

        }


        public static BaseNode? ConvertNode(this BaseNodeDTO dto, ServiceBundle SB)
        {
            dynamic? type;
            if (!dto.Properties.TryGetValue(PropertyType.Type, out type))
            {
                type = dto.NodeClass ?? string.Empty;
            }
            switch (type)
            {

                // Math
                case "SummationNode":
                    return new SummationNode(dto.Guid, dto.Position);
                case "SineNode":
                    return new SineNode(dto.Guid, dto.Position);
                case "RandomNode":
                    return new RandomNode(dto.Guid, dto.Position);
                case "MinNode":
                    return new MinNode(dto.Guid, dto.Position);
                case "MaxNode":
                    return new MaxNode(dto.Guid, dto.Position);
                case "CounterNode":
                    return new CounterNode(dto.Guid, dto.Position);
                case "AverageNode":
                    return new AverageNode(dto.Guid, dto.Position);
                case "SubtractNode":
                    return new SubtractNode(dto.Guid, dto.Position);
                case "MultiplicationNode":
                    return new MultiplicationNode(dto.Guid, dto.Position);
                case "DivisionNode":
                    return new DivisionNode(dto.Guid, dto.Position);
                case "AddNode":
                    return new AddNode(dto.Guid, dto.Position);
                case "AbsoluteNode":
                    return new AbsoluteNode(dto.Guid, dto.Position);
                case "ClampNode":
                    return new ClampNode(dto.Guid, dto.Position);
                case "RollingAverageNode":
                    return new RollingAverageNode(dto.Guid, dto.Position);
                case "SquareRootNode":
                    return new SquareRootNode(dto.Guid, dto.Position);

                // Logic
                case "AndNode":
                    return new AndNode(dto.Guid, dto.Position);
                case "OrNode":
                    return new OrNode(dto.Guid, dto.Position);
                case "XOrNode":
                    return new XOrNode(dto.Guid, dto.Position);
                case "NotNode":
                    return new NotNode(dto.Guid, dto.Position);
                case "EqualNode":
                    return new EqualNode(dto.Guid, dto.Position);
                case "GreaterThanNode":
                    return new GreaterThanNode(dto.Guid, dto.Position);
                case "LessThanNode":
                    return new LessThanNode(dto.Guid, dto.Position);
                case "GreaterThanOrEqualNode":
                    return new GreaterThanEqualNode(dto.Guid, dto.Position);
                case "LessThanOrEqualNode":
                    return new LessThanEqualNode(dto.Guid, dto.Position);

                // Constants
                case "NumericNode":
                    return new NumericNode(dto.Guid, dto.Value, dto.Position);
                case "LogicNode":
                    return new LogicNode(dto.Guid, dto.Value, dto.Position);

                // Activity
                case "PulsoidNode":
                    return new PulsoidNode(dto.Guid, SB, dto.Position);

                // OSC
                case "OSCBooleanNode":
                    return new OSCBooleanNode(dto.Guid, dto.Option, SB, dto.Position);
                case "OSCBooleanReadNode":
                    return new OSCBooleanReadNode(dto.Guid, dto.Option, SB, dto.Position);
                case "OSCIntNode":
                    return new OSCIntNode(dto.Guid, dto.Option, SB, dto.Position);
                case "OSCIntReadNode":
                    return new OSCIntReadNode(dto.Guid, dto.Option, SB, dto.Position);
                case "OSCFloatNode":
                    return new OSCFloatNode(dto.Guid, dto.Option, SB, dto.Position);
                case "OSCFloatReadNode":
                    return new OSCFloatReadNode(dto.Guid, dto.Option, SB, dto.Position);

                // VRChat 
                case "AvatarParameterBooleanNode":
                    return new AvatarParameterBooleanNode(dto.Guid, dto.Option, SB, dto.Position);
                case "AvatarParameterBooleanReadNode":
                    return new AvatarParameterBooleanReadNode(dto.Guid, dto.Option, SB, dto.Position);
                case "AvatarParameterIntNode":
                    return new AvatarParameterIntNode(dto.Guid, dto.Option, SB, dto.Position);
                case "AvatarParameterIntReadNode":
                    return new AvatarParameterIntReadNode(dto.Guid, dto.Option, SB, dto.Position);
                case "AvatarParameterFloatNode":
                    return new AvatarParameterFloatNode(dto.Guid, dto.Option, SB, dto.Position);
                case "AvatarParameterFloatReadNode":
                    return new AvatarParameterFloatReadNode(dto.Guid, dto.Option, SB, dto.Position);

                case "OSCVRCAvatarFloatReadNode":
                    return new OSCVRCAvatarFloatReadNode(dto.Guid, dto.Option, SB, dto.Position);
                case "OSCVRCAvatarIntReadNode":
                    return new OSCVRCAvatarIntReadNode(dto.Guid, dto.Option, SB, dto.Position);
                case "OSCVRCAvatarBooleanReadNode":
                    return new OSCVRCAvatarBooleanReadNode(dto.Guid, dto.Option, SB, dto.Position);
                case "OSCVRCAxisNode":
                    return new OSCVRCAxisNode(dto.Guid, dto.Option, SB, dto.Position);
                case "OSCVRCButtonNode":
                    return new OSCVRCButtonNode(dto.Guid, dto.Option, SB, dto.Position);
                case "OSCVRCChatboxNode":
                    return new OSCVRCChatboxNode(dto.Guid, SB, dto.Position);

                // Utility Nodes
                case "LogicSwitchNode":
                    return new LogicSwitchNode(dto.Guid, dto.Position);
                case "NumericSwitchNode":
                    return new NumericSwitchNode(dto.Guid, dto.Position);
                case "DelayNode":
                    return new DelayNode(dto.Guid, dto.Properties, dto.Position);


                default:
                    return null;
            }
        }
    }
}
