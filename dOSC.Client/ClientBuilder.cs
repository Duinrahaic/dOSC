using dOSC.Client.Services;
using dOSC.Client.Services.Connectors.Hub.Activity.Pulsoid;
using dOSC.Client.Services.Connectors.Hub.OSC;
using dOSC.Shared.Utilities;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;

namespace dOSC.Client;

public static class ClientBuilder
{
    public static void AddServices(this IServiceCollection services)
    {
        GlobalTimer.Initialize();
        VisualUpdateTimer.Initialize();

        services.AddSingleton<dOSCService>();
        services.AddHostedService(sp => sp.GetRequiredService<dOSCService>());
        
        services.AddSingleton<WebsocketClient>();

        // Remove this later
        services.AddSingleton<OSCService>();
        services.AddSingleton<PulsoidService>();
        services.AddSingleton<ServiceBundle>();

        services.AddHostedService(sp => sp.GetRequiredService<OSCService>());

        services.AddHostedService(sp => sp.GetRequiredService<PulsoidService>());
    }
    
    public static void AddServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddServices();
    }

    public static void AddLogging(this WebAssemblyHostBuilder builder, string applicationName = "")
    {
        builder.Services.AddLogging(applicationName);
    }
    
    public static void AddLogging(this IServiceCollection services ,string applicationName = "")
    {
        services.AddLogging(logging =>
        {
            logging.AddSerilog(LoggerProvider.SetupLogger(applicationName, LogPool.Sink), true);
        });
    }
}