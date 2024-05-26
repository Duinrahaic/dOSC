using dOSC.Client.Services;
using dOSC.Client.Services.Connectors.Hub.Activity.Pulsoid;
using dOSC.Client.Services.Connectors.Hub.OSC;
using dOSC.Shared.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace dOSC.Client;

public static class ClientBuilder
{
    public static void AddServices(this IServiceCollection services)
    {
        GlobalTimer.Initialize();
        VisualUpdateTimer.Initialize();

        //services.AddSingleton<dOSCService>();
        //services.AddHostedService(sp => sp.GetRequiredService<dOSCService>());
        
 
        // Remove this later
        services.AddSingleton<OSCService>();
        services.AddSingleton<PulsoidService>();
        //services.AddSingleton<ServiceBundle>();

        services.AddHostedService(sp => sp.GetRequiredService<OSCService>());

        services.AddHostedService(sp => sp.GetRequiredService<PulsoidService>());
    }
    
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddServices();
    }

    public static void AddLogging(this WebApplicationBuilder builder, string applicationName = "")
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