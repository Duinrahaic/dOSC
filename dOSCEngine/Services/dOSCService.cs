using Blazor.Diagrams;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Services.Connectors.Activity.Pulsoid;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace dOSCEngine.Services
{
    public partial class dOSCService : IHostedService
    {
        private ConcurrentDictionary<DateTime, string> _Log = new ConcurrentDictionary<DateTime, string>(2, capacity:200);
        public ConcurrentDictionary<DateTime,string> Log => _Log;
        public readonly string Version = "1";
        private readonly ILogger<OSCService> _logger;
        private readonly OSCService? _OSC;
        private readonly PulsoidService? _Pulsoid;
        private List<dOSCWiresheet> _WiresheetMemory = new();
        public dOSCWiresheet? ActiveWiresheet { get; set; }
        public BlazorDiagram _diagram { get; set; } = null!;

        // Write a subcription event for on wiresheet change
        public Action<dOSCWiresheet?>? OnWiresheetChange;

        public dOSCService(IServiceProvider services)
        {
            _logger = services.GetService<ILogger<OSCService>>()!;
            _logger.LogInformation("Initialized OSCService");
            _OSC = services.GetService<OSCService>();
            _Pulsoid = services.GetService<PulsoidService>();
            LoadWiresheets();

            //_WiresheetMemory.ForEach(w => w.Start());
        }

        public List<dOSCWiresheet> GetWireSheets()
        {
            return _WiresheetMemory;
        }

        public dOSCWiresheet? GetWiresheet(Guid AppId)
        {
            return _WiresheetMemory.FirstOrDefault(x => x.AppGuid.Equals(AppId));
        }

        private void AddLog(DateTime time, string message)
        {
            _Log.TryAdd(time, message);
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
