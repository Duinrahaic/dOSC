using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Ports;
using dOSC.Services.Connectors.OSC;

namespace dOSC.Engine.Nodes.Connector.OSC
{
    public class OSCVRCChatboxNode : BaseNode
    {
        public OSCVRCChatboxNode(OSCService ? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {

            AddPort(new LogicPort(this, true)); // Send
            AddPort(new StringPort(this, true)); // Message 
            AddPort(new LogicPort(this, true)); // Immediately?
            AddPort(new LogicPort(this, true)); // SFX?
            AddPort(new LogicPort(this, true)); // typing?

            _service = service;
        }
        private readonly OSCService? _service = null;
        public override string BlockTypeClass => "connectorblock";

        private bool MessageSent = false;

        public override void Refresh()
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
                if(typing.Links.Any())
                {
                    var i = GetInputValue(typing, typing.Links.First());
                    var v = Convert.ToInt32(i);
                    _service.SendMessage(OSCService.ChatboxTyping, v);
                }
            }
            
            base.Refresh();
        }

        


    }
}
