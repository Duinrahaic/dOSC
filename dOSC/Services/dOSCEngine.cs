using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Nodes;
using dOSC.Engine.Nodes.Connector.OSC;
using dOSC.Engine.Nodes.Constant;
using dOSC.Engine.Nodes.Logic;
using dOSC.Engine.Nodes.Math;
using dOSC.Engine.Nodes.Utility;
using dOSC.Engine.Ports;
using dOSC.Services.Connectors.Activity.Pulsoid;
using dOSC.Services.Connectors.OSC;
using dOSC.Utilities;

namespace dOSC.Services
{
    public partial class dOSCEngine : IHostedService
    {
        public readonly string Version = "1";
        private readonly ILogger<OSCService> _logger;
        private readonly OSCService? _OSC;
        private readonly PulsoidService? _Pulsoid;
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
            _Pulsoid = services.GetService<PulsoidService>();
            LoadWiresheets();
            _WiresheetMemory.ForEach(w => w.Start());
        }

        public List<dOSCWiresheet> GetWireSheets()
        {
            return _WiresheetMemory;
        }

        public dOSCWiresheet? GetWiresheet(Guid AppId)
        {
            return _WiresheetMemory.FirstOrDefault(x => x.AppGuid.Equals(AppId));
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


        
    }
}
