using dOSCEngine.Services.Connectors.Activity.Pulsoid;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Services;
using Serilog;
using dOSCEngine.Utilities;
using System.Net.Sockets;
using System.Net;
using dOSCHub;
using dOSCHub.Services;
using Grpc.Core;

namespace dOSC
{
    public static class Webserver
    {
        public static bool IsRunning { get; private set; }
        private static WebApplication app;

        public static List<string> GetUrls() => IsRunning ? app.Urls.ToList() : new();
  
        public static void Start(string[] args)
        {
            
            
            
            var builder = WebApplication.CreateBuilder();

            builder.WebHost.UseStaticWebAssets();
            builder.Services.AddSingleton<IHostedService, dOSCHubService>();
            
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            builder.Services.AddSingleton<OSCService>();
            builder.Services.AddSingleton<dOSCService>();
            builder.Services.AddSingleton<PulsoidService>();
            builder.Services.AddSingleton<ServiceBundle>();
            
            builder.Services.AddHostedService(sp => sp.GetRequiredService<OSCService>());
            builder.Services.AddHostedService(sp => sp.GetRequiredService<PulsoidService>());
            builder.Services.AddHostedService(sp => sp.GetRequiredService<dOSCService>());
            
            
            
            GlobalTimer.Initialize();
            VisualUpdateTimer.Initialize();
            var Serilog = new LoggerConfiguration()
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
                logging.AddSerilog(logger: Serilog, dispose: true);
            });

            app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseStaticFiles();
            app.UseRouting();
            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");
            IsRunning = true;



			string url = $@"https://localhost:{FreeTcpPort()}";
#if DEBUG
            url = $@"https://localhost:5231";
#endif

            if (!args.Any(x => x.ToLower().Equals("--headless")))
			{
				app.RunAsync(url);
			}
			else
			{
				app.Run(url);
			}
		}


		private static int FreeTcpPort()
		{
			TcpListener l = new TcpListener(IPAddress.Loopback, 0);
			l.Start();
			int port = ((IPEndPoint)l.LocalEndpoint).Port;
			l.Stop();
			return port;
		}


		public static void Stop()
        {
            IsRunning = false;
            app.StopAsync();
        }
    }
}
