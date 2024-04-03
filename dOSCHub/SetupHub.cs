using dOSCEngine.Services;
using dOSCEngine.Services.Connectors.Activity.Pulsoid;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Utilities;
using Serilog;

namespace dOSCHub;

public static class Setup
{
    private static WebApplication? _app;
    public static bool IsRunning { get; private set; }


 		
    public static List<string> GetUrls() => IsRunning ? _app?.Urls.ToList() ??new() : new();

    public static async Task Start(string[] args)
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddGrpc();
        
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        builder.Services.AddSingleton<OSCService>();
        builder.Services.AddSingleton<dOSCService>();
        builder.Services.AddSingleton<PulsoidService>();
        builder.Services.AddSingleton<ServiceBundle>();
        
        builder.Services.AddHostedService(sp => sp.GetRequiredService<OSCService>());
        builder.Services.AddHostedService(sp => sp.GetRequiredService<PulsoidService>());
        builder.Services.AddHostedService(sp => sp.GetRequiredService<dOSCService>());
        
        var serilog = new LoggerConfiguration()
            .WriteTo.Async(writeTo => writeTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff}[{Level}] {Message}{NewLine}{Exception}"))
            .WriteTo.Async(writeTo => writeTo.File(
                Path.Combine(FileSystem.LogFolder, "dOSC"),
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff}[{Level}] {Message}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 200000000,
                buffered: true
            ))
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Services.AddLogging(logging =>
        {
            logging.AddSerilog(logger: serilog, dispose: true);
        });

        builder.Services.AddGrpc();
        
        _app = builder.Build();
        
        _app.UseGrpcWeb();
        
        // Configure the HTTP request pipeline.
        if (!_app.Environment.IsDevelopment())
        {
            _app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            _app.UseHsts();
        }


        _app.UseStaticFiles();
        _app.UseRouting();
        _app.MapBlazorHub();
        _app.MapFallbackToPage("/_Host");
        IsRunning = true;

        var settings = FileSystem.LoadSettings();
		string url = $@"http://localhost:{settings?.dOSC.GetHubServerPort() ?? 5232}";
#if DEBUG
        url = $@"http://localhost:5232";
#endif

        if (!args.Any(x => x.Equals("--headless", StringComparison.CurrentCultureIgnoreCase )))
		{
			await _app.RunAsync(url);
		}
		else
		{
			_app.Run(url);
		}
	}





	public static void Stop()
    {
        IsRunning = false;
        _app?.StopAsync();
    }
}