using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Connector.VRChat
{
    public class OSCVRCChatboxNode : BaseNode
    {
        public OSCVRCChatboxNode(OSCService? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {

            AddPort(new LogicPort(PortGuids.Port_1, this, true)); // Send
            AddPort(new StringPort(PortGuids.Port_2, this, true)); // Message 
            AddPort(new LogicPort(PortGuids.Port_3, this, true)); // Immediately?
            AddPort(new LogicPort(PortGuids.Port_4, this, true)); // SFX?
            AddPort(new LogicPort(PortGuids.Port_5, this, true)); // typing?

            _service = service;
        }
        public OSCVRCChatboxNode(Guid guid, OSCService? service = null, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, true)); // Send
            AddPort(new StringPort(PortGuids.Port_2, this, true)); // Message 
            AddPort(new LogicPort(PortGuids.Port_3, this, true)); // Immediately?
            AddPort(new LogicPort(PortGuids.Port_4, this, true)); // SFX?
            AddPort(new LogicPort(PortGuids.Port_5, this, true)); // typing?
            _service = service;
        }
        private readonly OSCService? _service = null;
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        public override string BlockTypeClass => "connectorblock";

        private bool MessageSent = false;

        public override void CalculateValue()
        {
            if (_service != null)
            {
                var send = Ports.First();
                if (send.Links.Any())
                {
                    var sv = GetInputValue(send, send.Links.First());
                    if (sv != null)
                    {
                        if (MessageSent != (bool)sv)
                        {
                            MessageSent = (bool)sv;
                            if (MessageSent)
                            {
                                var message = Ports[1];
                                var immediately = Ports[2];
                                var sfx = Ports[3];
                                dynamic? mv = null;
                                dynamic? iv = null;
                                dynamic? sfxv = null;
                                if (message.Links.Any())
                                {
                                    mv = GetInputValue(message, message.Links.First());
                                }
                                if (immediately.Links.Any())
                                {
                                    iv = GetInputValue(immediately, immediately.Links.First());
                                }
                                if (sfx.Links.Any())
                                {
                                    sfxv = GetInputValue(sfx, sfx.Links.First());
                                }
                                if (!string.IsNullOrEmpty(mv))
                                {
                                    _service.SendChatMessage(mv, iv ?? false, sfxv ?? false);
                                }
                            }
                        }
                    }
                }
                var typing = Ports.Last();
                if (typing.Links.Any())
                {
                    var i = GetInputValue(typing, typing.Links.First());
                    var v = Convert.ToInt32(i);
                    _service.SendMessage(OSCService.ChatboxTyping, v);
                }
            }

        }
    }
}
