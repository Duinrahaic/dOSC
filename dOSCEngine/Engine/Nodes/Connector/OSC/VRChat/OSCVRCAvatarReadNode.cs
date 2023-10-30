using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Connector.OSC.VRChat
{
    public class OSCVRCAvatarReadNode : BaseNode
    {


        public OSCVRCAvatarReadNode(OSCService? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            int portNumber = 1;
            foreach (var _ in BoolOptions)
            {
                AddPort(new LogicPort(PortGuids.PortGuidGenerator(portNumber), this, false));
                portNumber++;
            }
            foreach (var _ in IntOptions)
            {
                AddPort(new NumericPort(PortGuids.PortGuidGenerator(portNumber), this, false));
                portNumber++;
            }
            foreach (var _ in FloatOptions)
            {
                AddPort(new NumericPort(PortGuids.PortGuidGenerator(portNumber), this, false));
                portNumber++;
            }

            Options.AddRange(BoolOptions);
            Options.AddRange(IntOptions);
            Options.AddRange(FloatOptions);
            _service = service;
            if (_service != null)
            {
                _service.OnOSCMessageRecieved += OnMessageRecieved;
            }
        }
        public OSCVRCAvatarReadNode(Guid guid, OSCService? service = null, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            int portNumber = 1;
            foreach (var _ in BoolOptions)
            {
                AddPort(new LogicPort(PortGuids.PortGuidGenerator(portNumber), this, false));
                portNumber++;
            }
            foreach (var _ in IntOptions)
            {
                AddPort(new NumericPort(PortGuids.PortGuidGenerator(portNumber), this, false));
                portNumber++;
            }
            foreach (var _ in FloatOptions)
            {
                AddPort(new NumericPort(PortGuids.PortGuidGenerator(portNumber), this, false));
                portNumber++;
            }
            Options.AddRange(BoolOptions);
            Options.AddRange(IntOptions);
            Options.AddRange(FloatOptions);
            _service = service;
            if (_service != null)
            {
                _service.OnOSCMessageRecieved += OnMessageRecieved;
            }
        }
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        public override string BlockTypeClass => "connectorblock";

        private readonly OSCService? _service = null;
        public List<string> Options = new();
        private List<string> BoolOptions = new()
        {
            OSCService.InStation,
            OSCService.Seated,
            OSCService.AFK,
            OSCService.MuteSelf
        };
        private List<string> FloatOptions = new()
        {
            OSCService.VelocityZ,
            OSCService.VelocityX,
            OSCService.VelocityY,
            OSCService.Upright,
            OSCService.AngularY,
            OSCService.GestureLeftWeight,
            OSCService.GestureRightWeight,
        };
        private List<string> IntOptions = new()
        {
            OSCService.Face,
            OSCService.Viseme,
            OSCService.TrackingType,
            OSCService.GestureLeft,
            OSCService.GestureRight,
        };

        private Dictionary<string, dynamic> _data = new Dictionary<string, dynamic>();
        private void OnMessageRecieved(OSCSubscriptionEvent e)
        {
            if (Options.Any(x => x.ToLower().Equals(e.Address.ToLower())))
            {
                bool contains = _data.ContainsKey(e.Address);
                if (!contains)
                {
                    _data[e.Address] = e.Arguments.First();
                }
                else
                {
                    _data.TryAdd(e.Address, e.Arguments.First());
                }

            }
        }
    }
}
