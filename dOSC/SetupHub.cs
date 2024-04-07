using dOSC.Drivers.DB;
using dOSC.Drivers.DB.Models;
using dOSC.Drivers.OSC;
using dOSC.Drivers.Pulsoid;
using dOSC.Middlewear;
using dOSC.Shared.Utilities;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace dOSC;

public static class SetupHub
{
    private static WebApplication? _app;
    public static bool IsRunning { get; private set; }

    public static List<string> GetUrls()
    {
        return IsRunning ? _app?.Urls.ToList() ?? new List<string>() : new List<string>();
    }

    public static async void Start(string[] args)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            { ContentRootPath = Directory.GetCurrentDirectory(), ApplicationName = "dOSC" });
        dOSCFileSystem.CreateFolders();

        builder.WebHost.UseStaticWebAssets();
#if DEBUG
        var port = 5232;
#else
            var settings = dOSCFileSystem.LoadSettings();
	        int port = settings?.dOSC.GetHubServerPort() ?? 5232;
#endif
        var url = $@"http://localhost:{port}";

        builder.WebHost.UseKestrel();
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenLocalhost(port, o => o.Protocols =
                HttpProtocols.Http1);
        });

        builder.AddHubServices();

        builder.AddLogging("dOSCHub");


        _app = builder.Build();

        // Setup the HTTP request pipeline.
        if (!_app.Environment.IsDevelopment())
        {
            _app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            _app.UseHsts();
        }

        _app.UseWebSockets();
        _app.UseMiddleware<WebSocketMiddleware>();
        IsRunning = true;

        _app.Urls.Add(url);
        if (!args.Any(x => x.Equals("--headless", StringComparison.CurrentCultureIgnoreCase)))
            await _app.RunAsync();
        else
            _app.Run();
    }


    public static void Stop()
    {
        IsRunning = false;
        _app?.StopAsync();
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
}