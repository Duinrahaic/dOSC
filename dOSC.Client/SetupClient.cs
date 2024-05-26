using Avalonia;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace dOSC.Client;
public static class SetupClient
{    
    public static void Start(string[] args)
    {
        var appBuilder = Host.CreateApplicationBuilder(args);
        appBuilder.Services.AddWindowsFormsBlazorWebView();
        appBuilder.Services.AddBlazorWebViewDeveloperTools();
        appBuilder.Services.AddServices();
        appBuilder.Services.AddLogging("dOSCClient");
        using var myApp = appBuilder.Build();

        myApp.Start();
        
        try
        {
            BuildAvaloniaApp(myApp.Services)                
                .StartWithClassicDesktopLifetime(args);

        }
        finally
        {
            Task.Run(async () => await myApp.StopAsync()).Wait();
        }
    }
    
    
    private static AppBuilder BuildAvaloniaApp(IServiceProvider serviceProvider)
        => AppBuilder.Configure<dOSC.Client.Desktop.App>(() => new (serviceProvider))
            .UsePlatformDetect()
            .LogToTrace()
            //.UseManagedSystemDialogs()
            .UseReactiveUI();

    public static AppBuilder BuildAvaloniaApp() => BuildAvaloniaApp(null!);

}