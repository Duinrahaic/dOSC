using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Nodes;
using dOSC.Engine.Nodes.Connector.OSC;
using dOSC.Engine.Nodes.Constant;
using dOSC.Engine.Nodes.Logic;
using dOSC.Engine.Nodes.Math;
using dOSC.Engine.Nodes.Utility;
using dOSC.Engine.Ports;
using dOSC.Services.Connectors.OSC;
using dOSC.Utilities;

namespace dOSC.Services
{
    public class dOSCEngine : IHostedService
    {
        public readonly string Version = "1";
        private readonly ILogger<OSCService> _logger;
        private readonly OSCService? _OSC;
        private List<dOSCWiresheet> _WiresheetMemory = new();
        public dOSCWiresheet? ActiveWiresheet { get; set; }
        public BlazorDiagram _diagram { get; set; } = null!;

        // Write a subcription event for on wiresheet change
        public Action<dOSCWiresheet?>? OnWiresheetChange;

        public dOSCEngine(IServiceProvider services)
        {
            _logger = services.GetService<ILogger<OSCService>>()!;
            _logger.LogInformation("Initialized OSCService");
            _OSC = services.GetService<OSCService>();


            

            dOSCWiresheet wsA = new();
            dOSCWiresheet wsB = new();

            var nn = new NumericNode(position: new Point(220, 200));
            var en = new EqualNode(position: new Point(400, 400));
            wsA.AddNode(new AddNode(position: new Point(210, 200)));
            wsA.AddNode(nn);
            wsA.AddNode(new OSCVRCAxisNode(service:_OSC, position: new Point(230, 200)));
            wsA.AddNode(new NumericNode(position: new Point(230, 200)));
            wsA.AddNode(new NumericNode(position: new Point(230, 200)));
            wsA.AddNode(new SineNode(position: new Point(230, 200)));
            wsA.AddNode(new BooleanNode(position: new Point(300, 300)));
            wsA.AddNode(new BooleanNode(position: new Point(300, 300)));
            wsA.AddNode(en);


            var source = nn.Ports.First();
            var target = en.Ports.First();

            wsA.AddRelationship((source as BasePort), (target as BasePort));


            wsA.Build();
            wsB.Build();

            wsA.Start();
            wsB.Start();
            
            AddWiresheet(wsA);
            AddWiresheet(wsB);
            SaveWiresheet(wsA);
            LoadWiresheets();

            _WiresheetMemory.ForEach(x=>x.Build());
            _WiresheetMemory.ForEach(x=>x.Start());
        }

        public List<dOSCWiresheet> GetWireSheets()
        {
            return _WiresheetMemory;
        }

        public dOSCWiresheet GetActiveWiresheet()
        {
            return ActiveWiresheet;
        }


        public void SetActiveWiresheet(Guid AppGuid)
        {
            dOSCWiresheet? WS = _WiresheetMemory.FirstOrDefault(x=>x.AppGuid.Equals(AppGuid));
            if(WS == null)
            {
                throw new Exception("Wiresheet Not Found");
            }
            ActiveWiresheet = WS;
            OnWiresheetChange?.Invoke(ActiveWiresheet);
        }

        public void SetActiveWiresheet(dOSCWiresheet WS)
        {
            if(WS==null)
            {
                throw new Exception("Wiresheet is null");
            }
            ActiveWiresheet = WS;
            OnWiresheetChange?.Invoke(ActiveWiresheet);
        }

        public void UnsetActiveWiresheet()
        {
            ActiveWiresheet = null;
            OnWiresheetChange?.Invoke(ActiveWiresheet);
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

        public dOSCWiresheet? GetWiresheet(Guid AppId)
        {
            return _WiresheetMemory.FirstOrDefault(x => x.AppGuid.Equals(AppId));
        }

        public void SaveWiresheet(dOSCWiresheet WS)
        {
            try
            {
                FileSystem.SaveWiresheet(WS);
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

                    _WiresheetMemory.AddRange(ws.Select(x => DeserializeDTO(x)));
                }

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Unable to load wiresheets: {ex}");
            }
        }

        




        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                FileSystem.CreateFolders();
                _ = FileSystem.LoadSettings();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Unable to create file system folders: {ex}");
            }
            return Task.CompletedTask;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
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
                    return new BooleanNode(dto.Guid, dto.Value, dto.Position);
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
