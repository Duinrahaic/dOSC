using dOSC.Services.ElectronFramework;
using dOSC.Services.Connectors.OSC;
using Serilog;
using dOSC.Utilities;
using dOSC.Services;
using Serilog.Sanitizer.Extensions;
using dOSC.Services.Connectors.Activity.Pulsoid;

namespace dOSC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var Serilog = new LoggerConfiguration()
                .WriteTo.Async(writeTo => writeTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff}[{Level}] {Message}{NewLine}{Exception}"))
                .WriteTo.Async(writeTo => writeTo.File(
                    Path.Combine(FileSystem.LogFolder,"dOSC"), 
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff}[{Level}] {Message}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 200000000,
                    buffered: true
                ))
                .Enrich.FromLogContext()
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<OSCService>();
            builder.Services.AddSingleton<dOSCEngine>();
            builder.Services.AddSingleton<PulsoidService>();
            builder.Services.AddHostedService(sp => sp.GetRequiredService<OSCService>());
            builder.Services.AddHostedService(sp => sp.GetRequiredService<PulsoidService>());
            builder.Services.AddHostedService(sp => sp.GetRequiredService<dOSCEngine>());
            builder.Services.AddLogging(logging =>
            {
                logging.AddSerilog(logger: Serilog, dispose: true);
            });
            //builder.Logging.AddSerilog(logger: Serilog, dispose: true) ;
            string ENV = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";
            if (ENV != "Development")
            {
                builder.LaunchElectronWindow(args);
            }

            var app = builder.Build();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");
            
            GlobalStopwatch.Start();
            GlobalTimer.StartGlobalTimer();
            app.Run();

        }


    }
}







