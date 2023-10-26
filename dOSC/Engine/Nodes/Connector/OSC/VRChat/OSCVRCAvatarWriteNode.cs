using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Ports;
using dOSC.Services.Connectors.OSC;
using Newtonsoft.Json;

namespace dOSC.Engine.Nodes.Connector.OSC.VRChat
{
    public class OSCVRCAvatarWriteNode : BaseNode
    {
        public OSCVRCAvatarWriteNode(OSCService? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true)); // Viseme
            _service = service;
        }
        public OSCVRCAvatarWriteNode(Guid guid, OSCService? service = null, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true)); // Viseme
            _service = service;
        }
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
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
