using System.Net.Sockets;
using System.Net;
using Microsoft.Extensions.Hosting;
using dOSCEngine.Services.Connectors.OSC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using XSNotifications;

namespace dOSCEngine.Services.Connectors.Overlay.XSOverlay
{
    public class XSOverlayService : IHostedService, IDisposable
    {
        private const int Port = 42069;
        private readonly ILogger<XSOverlayService> _logger;

 

        public XSOverlayService(IServiceProvider services)
        {
            _logger = services.GetService<ILogger<XSOverlayService>>()!;
            _logger.LogInformation("Initialized XSOverlayService");
            StartService();
        }
        
        public void StartService() {
        
    
        }

        public void StopService()
        {
        }

   


        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
