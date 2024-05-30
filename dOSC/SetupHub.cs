using System.Runtime.ExceptionServices;
using Avalonia;
using Avalonia.ReactiveUI;
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
    private static WebApplication? app;
    public static bool IsRunning { get; private set; }

    public static List<string> GetUrls()
    {
        return IsRunning ? app?.Urls.ToList() ?? new List<string>() : new List<string>();
    }

    public static void Start(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

         
#if DEBUG
        var port = 5232;
#else
            var settings = dOSCFileSystem.LoadSettings();
	        int port = settings?.dOSC.GetHubServerPort() ?? 5232;
#endif
        var url = $@"http://localhost:{port}";

        
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenLocalhost(port, o => o.Protocols =
                HttpProtocols.Http1);
        });

        builder.AddHubServices();

        builder.AddLogging("dOSCHub");


        app = builder.Build();

        // Setup the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseWebSockets();
        app.UseMiddleware<WebSocketMiddleware>();
        IsRunning = true;

        app.Urls.Add(url);
        app.Start();


        try
        {
            App.RunAvaloniaAppWithHosting(args, BuildAvaloniaApp); // Builds WebView
        }
        catch(Exception ex)
        {
            ExceptionDispatchInfo.Capture(ex).Throw();
        }
        finally
        {
            Task.Run(async () => await app.StopAsync()).Wait();
        } 
    }
    
    public static void Stop()
    {
        IsRunning = false;
        app?.StopAsync();
    }

    private static WebApplicationBuilder AddHubServices(this WebApplicationBuilder builder)
    {
        using (var client = new DBEntities())
        {
            client.Database.EnsureCreated();
            client.Database.Migrate();
        }

        builder.Services.AddScoped<dOSCDBService>();
         
        
        builder.Services.AddSingleton<OSCService>();
        builder.Services.AddSingleton<PulsoidService>();

        builder.Services.AddHostedService(sp => sp.GetRequiredService<OSCService>());
        builder.Services.AddHostedService(sp => sp.GetRequiredService<PulsoidService>());
        
        return builder;
    }

    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder, string applicationName = "")
    {
        builder.Services.AddLogging(logging =>
        {
            logging.AddSerilog(LoggerProvider.SetupLogger(applicationName, LogPool.Sink), true);
        });
        return builder;
    }
    
    

    
    
    private static AppBuilder BuildAvaloniaApp(IServiceProvider serviceProvider)
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            //.UseManagedSystemDialogs()
            .UseReactiveUI();

    public static AppBuilder BuildAvaloniaApp() => BuildAvaloniaApp(null!);
}