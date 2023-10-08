using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Ports;
using dOSC.Services.Connectors.OSC;

namespace dOSC.Engine.Nodes.Connector.OSC
{
    public class OSCVRCWriteNode : BaseNode
    {
        public OSCVRCWriteNode(OSCService? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(this,true)); // Viseme
            _service = service;
        }
        public override string BlockTypeClass => "connectorblock";

        private readonly OSCService? _service = null;
        public List<string> Options = new()
        {
            OSCService.Viseme,
        };

        public override void Refresh()
        {
            int i = 0;
            if (_service != null)
            {
                foreach (var p in Ports)
                {
                    if (p.Links.Any())
                    {
                        var v = Convert.ToInt32(GetInputValue(p, p.Links.First()));
                        _service.SendMessage(Options[i], v);

                    }
                    i++;
                }
            }
            base.Refresh();
        }


    }
}
