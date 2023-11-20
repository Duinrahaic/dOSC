using dOSCEngine.Services.Connectors.Activity.Pulsoid;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Services;
using Serilog;
using dOSCEngine.Utilities;

namespace dOSC
{
    public static class Webserver
    {
        private static bool _isRunning = false;
        public static bool IsRunning { get => _isRunning; }
        private static WebApplication app;

 

        public static List<string> GetUrls()
        {
            if(_isRunning){

                return app.Urls.ToList();
            }
            else
            {
                return new();
            }
        }


        public static void Start(string[] args)
        {
            var builder = WebApplication.CreateBuilder();

            builder.WebHost.UseStaticWebAssets();

            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            builder.Services.AddSingleton<OSCService>();
            builder.Services.AddSingleton<dOSCService>();
            builder.Services.AddSingleton<PulsoidService>();
            builder.Services.AddHostedService(sp => sp.GetRequiredService<OSCService>());
            builder.Services.AddHostedService(sp => sp.GetRequiredService<PulsoidService>());
            builder.Services.AddHostedService(sp => sp.GetRequiredService<dOSCService>());

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

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();
            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");
            _isRunning = true;


            if (!args.Any(x => x.ToLower().Equals("--headless")))
            {
                app.RunAsync();
            }
            else
            {
                app.Run();
            }
        }

        public static void Stop()
        {
            _isRunning = false;
            app.StopAsync();
        }
    }
}
