using dOSC.Services.ElectronFramework;
using dOSC.Services.Connectors.OSC;
using Serilog;
using dOSC.Utilities;
using dOSC.Services;
namespace dOSC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var Serilog = new LoggerConfiguration()
                .WriteTo.Async(writeTo => writeTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff}[{Level}] {Message}{NewLine}{Exception}"))
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<OSCService>();
            builder.Services.AddSingleton<dOSCEngine>();
            builder.Services.AddHostedService(sp => sp.GetRequiredService<OSCService>());
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







