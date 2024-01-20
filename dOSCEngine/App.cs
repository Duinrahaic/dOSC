using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Services.Connectors.Activity.Pulsoid;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using dOSCEngine.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using dOSCEngine.Utilities;

namespace dOSCEngine
{
    public static class App
    {
        public static WebApplicationBuilder AddDataServices(this WebApplicationBuilder builder)
        {

            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            builder.Services.AddSingleton<OSCService>();
            builder.Services.AddSingleton<dOSCService>();
            builder.Services.AddSingleton<PulsoidService>();
            builder.Services.AddHostedService(sp => sp.GetRequiredService<OSCService>());
            builder.Services.AddHostedService(sp => sp.GetRequiredService<PulsoidService>());
            builder.Services.AddHostedService(sp => sp.GetRequiredService<dOSCService>());
            builder.Services.AddSignalR();
            return builder;
        }
    }
}
