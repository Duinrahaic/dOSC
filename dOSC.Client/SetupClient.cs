using dOSC.Client.Services;
using dOSC.Client.Services.Connectors.Hub.Activity.Pulsoid;
using dOSC.Client.Services.Connectors.Hub.OSC;
using dOSC.Shared.Utilities;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;


namespace dOSC.Client;

public static class SetupClient
{
    public static async void Start(string[] args)
    {
        EncryptionHelper.SetEncryptionKey(Environment.MachineName + "dOSC");

        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        builder.AddServices();
        builder.AddLogging("dOSCClient");
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        var app = builder.Build();

        await app.RunAsync();
    }
}