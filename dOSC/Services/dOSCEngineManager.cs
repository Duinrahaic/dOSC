using dOSC.Engine.Nodes.Connector.OSC;
using dOSC.Engine.Nodes.Constant;
using dOSC.Engine.Nodes.Logic;
using dOSC.Engine.Nodes.Math;
using dOSC.Engine.Nodes.Utility;
using dOSC.Engine.Nodes;
using dOSC.Utilities;
using dOSC.Engine.Nodes.Connector.Activity;
using dOSC.Engine.Nodes.Connector.OSC.VRChat;

namespace dOSC.Services
{
    public partial class dOSCEngine
    {
        public void SaveWiresheet(dOSCWiresheet WS)
        {
            try
            {
                FileSystem.SaveWiresheet(WS);
                try
                {
                    AddWiresheet(WS);
                }
                catch
                {

                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Unable to save wiresheets: {ex}");

            }
        }

        public void LoadWiresheets()
        {
            try
            {
                var ws = FileSystem.LoadWiresheets();
                if (ws != null)
                {

                    foreach (var w in ws.Select(x => DeserializeDTO(x)))
                    {
                        AddWiresheet(w);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Unable to load wiresheets: {ex}");
            }
        }
        public void AddWiresheet(dOSCWiresheet WS)
        {
            dOSCWiresheet? WSM = _WiresheetMemory.FirstOrDefault(x => x.AppGuid.Equals(WS.AppGuid));
            if (WSM != null)
            {
                throw new Exception("Wiresheet Already Exists");
            }
            _WiresheetMemory.Add(WS);
        }

        public void RemoveWiresheet(dOSCWiresheet WS)
        {
            dOSCWiresheet? WSM = _WiresheetMemory.FirstOrDefault(x => x.AppGuid.Equals(WS.AppGuid));
            if (WSM != null)
            {
                _WiresheetMemory.Remove(WSM);
            }
            FileSystem.RemoveWiresheet(WS.AppGuid);
        }

        private dOSCWiresheet DeserializeDTO(dOSCWiresheetDTO dto)
        {
            dOSCWiresheet dOSCWiresheet = new dOSCWiresheet(dto);
            var cNodes = dto.Nodes.Select(x => ConvertNode(x)).Where(x => x != null);
            foreach (var n in cNodes)
            {
                if (n != null)
                {
                    dOSCWiresheet.AddNode(n);
                }
            }

            return dOSCWiresheet;
        }

        private BaseNode? ConvertNode(BaseNodeDTO dto)
        {
            switch (dto.NodeClass)
            {
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
                case "SquareRootNode":
                    return new SquareRootNode(dto.Guid, dto.Position);
                case "AndNode":
                    return new AndNode(dto.Guid, dto.Position);
                case "OrNode":
                    return new OrNode(dto.Guid, dto.Position);
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
                case "NumericNode":
                    return new NumericNode(dto.Guid, dto.Value, dto.Position);
                case "BooleanNode":
                    return new LogicNode(dto.Guid, dto.Value, dto.Position);
                case "PulsoidNode":
                    return new PulsoidNode(dto.Guid, _Pulsoid, dto.Position);
                case "OSCBooleanNode":
                    return new OSCBooleanNode(dto.Guid,dto.Option, _OSC, dto.Position);
                case "OSCIntNode":
                    return new OSCIntNode(dto.Guid, dto.Option, _OSC, dto.Position);
                case "OSCFloatNode":
                    return new OSCFloatNode(dto.Guid, dto.Option, _OSC, dto.Position);
                case "OSCVRCAvatarReadNode":
                    return new OSCVRCAvatarReadNode(dto.Guid, _OSC, dto.Position);
                case "OSCVRCAvatarWriteNode":
                    return new OSCVRCWriteNode(dto.Guid, _OSC, dto.Position);
                case "OSCVRCAxisNode":
                    return new OSCVRCAxisNode(dto.Guid, dto.Option, _OSC, dto.Position);
                case "OSCVRCButtonNode":
                    return new OSCVRCButtonNode(dto.Guid, dto.Option, _OSC, dto.Position);
                case "OSCVRCChatboxNode":
                    return new OSCVRCChatboxNode(dto.Guid, _OSC, dto.Position);
                default:
                    return null;

            }

        }
    }
}
