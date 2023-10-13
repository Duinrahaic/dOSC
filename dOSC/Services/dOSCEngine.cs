using Blazor.Diagrams;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using CoreOSC;
using dOSC.Components.Wiresheet.Blocks.Connectors.OSC.Button;
using dOSC.Components.Wiresheet.Blocks.Logic;
using dOSC.Components.Wiresheet.Blocks.Math;
using dOSC.Engine.Nodes;
using dOSC.Engine.Nodes.Connector.OSC;
using dOSC.Engine.Nodes.Constant;
using dOSC.Engine.Nodes.Logic;
using dOSC.Engine.Nodes.Math;
using dOSC.Engine.Nodes.Utility;
using dOSC.Services.Connectors.OSC;

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

            wsA.AddNode(new AddNode(position: new Point(210, 200)));
            wsA.AddNode(new SubtractNode(position: new Point(210, 200)));
            wsA.AddNode(new SummationNode(position: new Point(210, 200)));
            wsA.AddNode(new MultiplicationNode(position: new Point(210, 200)));
            wsA.AddNode(new DivisionNode(position: new Point(210, 200)));
            wsA.AddNode(new NumericNode(position: new Point(220, 200)));
            wsA.AddNode(new NumericNode(position: new Point(230, 200)));
            wsA.AddNode(new NumericNode(position: new Point(230, 200)));
            wsA.AddNode(new SineNode(position: new Point(230, 200)));
            wsA.AddNode(new OSCVRCAxisNode(service: _OSC, position: new Point(230, 200)));
            wsA.AddNode(new OSCVRCButtonNode(service: _OSC, position: new Point(230, 200)));
            wsA.AddNode(new BooleanNode(position: new Point(300, 300)));
            wsA.AddNode(new BooleanNode(position: new Point(300, 300)));
            wsA.AddNode(new EqualNode(position: new Point(400, 400)));

            wsA.Build();
            wsB.Build();

            wsA.Start();
            wsB.Start();
            
            AddWiresheet(wsA);
            AddWiresheet(wsB);
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

        public Task StartAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
