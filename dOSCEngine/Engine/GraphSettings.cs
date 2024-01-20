using Blazor.Diagrams;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Nodes.Connectors.Activity;
using dOSCEngine.Engine.Nodes.Connectors.OSC;
using dOSCEngine.Engine.Nodes.Variables;
using dOSCEngine.Engine.Nodes.Logic;
using dOSCEngine.Engine.Nodes.Mathematics;
using dOSCEngine.Engine.Nodes.Utility;
using dOSCEngine.Services;
using System.Collections.Concurrent;

namespace dOSCEngine.Engine
{
    public static class GraphSettings
    {
        public static TimeSpan UpdateInterval = TimeSpan.FromMilliseconds(100);
        
        
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
                EnableSnapping = false
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
            // Activity
            BD.RegisterComponent<PulsoidNode, DefaultBlock>();

            // OSC
            BD.RegisterComponent<OSCReadNode, DefaultBlock>();
            BD.RegisterComponent<OSCWriteNode, DefaultBlock>();

            // Constants
            BD.RegisterComponent<LogicNode, DefaultBlock>();
            BD.RegisterComponent<NumericNode, DefaultBlock>();

            // Logic
            BD.RegisterComponent<AndNode, DefaultBlock>();
            BD.RegisterComponent<EqualNode, DefaultBlock>();
            BD.RegisterComponent<GreaterThanNode, DefaultBlock>();
            BD.RegisterComponent<GreaterThanEqualNode, DefaultBlock>();
            BD.RegisterComponent<LessThanNode, DefaultBlock>();
            BD.RegisterComponent<LessThanEqualNode, DefaultBlock>();
            BD.RegisterComponent<NotNode, DefaultBlock>();
            BD.RegisterComponent<NotEqualNode, DefaultBlock>();
            BD.RegisterComponent<OrNode, DefaultBlock>();
            BD.RegisterComponent<XOrNode, DefaultBlock>();

            // Math
            BD.RegisterComponent<AbsoluteNode, DefaultBlock>();
            BD.RegisterComponent<AddNode, DefaultBlock>();
            BD.RegisterComponent<AverageNode, DefaultBlock>();
            BD.RegisterComponent<ClampNode, DefaultBlock>();
            BD.RegisterComponent<DivisionNode, DefaultBlock>();
            BD.RegisterComponent<MultiplicationNode, DefaultBlock>();
            BD.RegisterComponent<PowerNode, DefaultBlock>();
            BD.RegisterComponent<SineNode, DefaultBlock>();
            BD.RegisterComponent<SquareRootNode, DefaultBlock>();
            BD.RegisterComponent<SubtractNode, DefaultBlock>();

            // Utility
            BD.RegisterComponent<DelayNode, DefaultBlock>();
            BD.RegisterComponent<LogicSwitchNode, DefaultBlock>();
            BD.RegisterComponent<InvertNode, DefaultBlock>();
            BD.RegisterComponent<NoteNode, NoteBlock>();
            BD.RegisterComponent<NumericSwitchNode, DefaultBlock>();
            BD.RegisterComponent<RandomNode, DefaultBlock>();
        }


        public static BaseNode? ConvertNode(this BaseNodeDTO dto, ServiceBundle SB)
        {
            switch (dto.NodeClass)
            {
                // Math
                case "SineNode":
                    return new SineNode(dto.Guid, dto.Properties, dto.Position);
                case "AverageNode":
                    return new AverageNode(dto.Guid, dto.Properties, dto.Position);
                case "SubtractNode":
                    return new SubtractNode(dto.Guid, dto.Properties, dto.Position);
                case "MultiplicationNode":
                    return new MultiplicationNode(dto.Guid, dto.Properties, dto.Position);
                case "DivisionNode":
                    return new DivisionNode(dto.Guid, dto.Properties, dto.Position);
                case "AddNode":
                    return new AddNode(dto.Guid, dto.Properties, dto.Position);
                case "AbsoluteNode":
                    return new AbsoluteNode(dto.Guid, dto.Properties, dto.Position);
                case "ClampNode":
                    return new ClampNode(dto.Guid, dto.Properties, dto.Position);
                case "SquareRootNode":
                    return new SquareRootNode(dto.Guid, dto.Properties, dto.Position);
                case "PowerNode":
                    return new PowerNode(dto.Guid, dto.Properties, dto.Position);

                // Logic
                case "AndNode":
                    return new AndNode(dto.Guid, dto.Properties, dto.Position);
                case "OrNode":
                    return new OrNode(dto.Guid, dto.Properties, dto.Position);
                case "XOrNode":
                    return new XOrNode(dto.Guid, dto.Properties, dto.Position);
                case "NotNode":
                    return new NotNode(dto.Guid, dto.Properties, dto.Position);
                case "EqualNode":
                    return new EqualNode(dto.Guid, dto.Properties, dto.Position);
                case "NotEqualNode":
                    return new NotEqualNode(dto.Guid, dto.Properties, dto.Position);
                case "GreaterThanNode":
                    return new GreaterThanNode(dto.Guid, dto.Properties, dto.Position);
                case "LessThanNode":
                    return new LessThanNode(dto.Guid, dto.Properties, dto.Position);
                case "GreaterThanEqualNode":
                    return new GreaterThanEqualNode(dto.Guid, dto.Properties, dto.Position);
                case "LessThanEqualNode":
                    return new LessThanEqualNode(dto.Guid, dto.Properties, dto.Position);

                // Constants
                case "NumericNode":
                    return new NumericNode(dto.Guid, dto.Properties, dto.Position);
                case "LogicNode":
                    return new LogicNode(dto.Guid, dto.Properties, dto.Position);

                // Activity
                case "PulsoidNode":
                    return new PulsoidNode(dto.Guid, dto.Properties, dto.Position, SB);
                // OSC 
                case "OSCReadNode":
                    return new OSCReadNode(dto.Guid, dto.Properties, dto.Position, SB);
                case "OSCWriteNode":
                    return new OSCWriteNode(dto.Guid, dto.Properties, dto.Position, SB);

                // Utility Nodes
                case "DelayNode":
                    return new DelayNode(dto.Guid, dto.Properties, dto.Position);
                case "InvertNode":
                    return new InvertNode(dto.Guid, dto.Properties, dto.Position);
                case "LogicSwitchNode":
                    return new LogicSwitchNode(dto.Guid, dto.Properties, dto.Position);
                case "NoteNode":
                    return new NoteNode(dto.Guid, dto.Properties, dto.Position);
                case "NumericSwitchNode":
                    return new NumericSwitchNode(dto.Guid, dto.Properties, dto.Position);
                case "RandomNode":
                    return new RandomNode(dto.Guid, dto.Properties, dto.Position);
                default:
                    return null;
            }

            
        }




        private static ConcurrentDictionary<string, dynamic>? ConvertToProperty<T>(string PropertyName, string Value)
        {
            ConcurrentDictionary<string, dynamic>? Properties = new();

            Properties.TryAdd(PropertyName, Value);
            return Properties;
        }
    }
}
