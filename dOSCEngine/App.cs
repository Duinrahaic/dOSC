using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Services.Connectors.Activity.Pulsoid;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Photino.Blazor;
using dOSCEngine.Services;

namespace dOSCEngine
{
    public static class App
    {
        public static PhotinoBlazorAppBuilder AddDataServices(this PhotinoBlazorAppBuilder builder)
        {
            builder.Services.AddSingleton<OSCService>();
            builder.Services.AddSingleton<dOSCService>();
            builder.Services.AddSingleton<PulsoidService>();
            builder.Services.AddHostedService(sp => sp.GetRequiredService<OSCService>());
            builder.Services.AddHostedService(sp => sp.GetRequiredService<PulsoidService>());
            builder.Services.AddHostedService(sp => sp.GetRequiredService<dOSCService>());


            return builder;
        }
        public static WebApplicationBuilder AddDataServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<OSCService>();
            builder.Services.AddSingleton<dOSCService>();
            builder.Services.AddSingleton<PulsoidService>();
            builder.Services.AddHostedService(sp => sp.GetRequiredService<OSCService>());
            builder.Services.AddHostedService(sp => sp.GetRequiredService<PulsoidService>());
            builder.Services.AddHostedService(sp => sp.GetRequiredService<dOSCService>());


            return builder;
        }
    }
}
