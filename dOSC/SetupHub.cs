using System.Runtime.ExceptionServices;
using Avalonia;
using Avalonia.ReactiveUI;
using dOSC.Drivers;
using dOSC.Drivers.DB;
using dOSC.Drivers.DB.Models;
using dOSC.Drivers.OSC;
using dOSC.Drivers.Pulsoid;
using dOSC.Middlewear;
using dOSC.Shared.Utilities;
using dOSC.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace dOSC;

public static class SetupHub
{
    public static bool IsRunning { get; private set; }
    
    public static void Start(string[] args)
    {
        try
        {
            App.RunAvaloniaAppWithHosting(args, BuildAvaloniaApp); // Builds WebView
        }
        catch(Exception ex)
        {
            ExceptionDispatchInfo.Capture(ex).Throw();
        }

    }
    
   
    private static AppBuilder BuildAvaloniaApp(IServiceProvider serviceProvider)
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            //.UseManagedSystemDialogs()
            .UseReactiveUI();

    public static AppBuilder BuildAvaloniaApp() => BuildAvaloniaApp(null!);
}