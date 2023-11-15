using dOSC.Utilities;
using dOSCEngine;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Photino.Blazor;
using Serilog;
using BlazorContextMenu;
namespace dOSC
{

    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Photino

            var builder = PhotinoBlazorAppBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            

            FileSystem.CreateFolders();
            _ = FileSystem.LoadSettings();

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
        
            //var builder = WebApplication.CreateBuilder(args);
            //builder.Services.AddRazorPages();
            //builder.Services.AddServerSideBlazor();

            builder.AddDataServices();
            builder.Services.AddLogging(logging =>
            {
                logging.AddSerilog(logger: Serilog, dispose: true);
            });
            builder.Services.AddBlazorContextMenu();

            //string ENV = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";
            //if (ENV != "Development")
            //{
            //    builder.LaunchElectronWindow(args);
            //}
            
            var app = builder.Build();

           





            app.MainWindow.Centered = true;
            app.MainWindow.SetChromeless(false);
            app.MainWindow
                .SetTitle("dOSC"); //                   .SetIconFile("favicon.ico")
            #if (DEBUG)
                app.MainWindow.DevToolsEnabled = true;
                app.MainWindow.ContextMenuEnabled = true;
            #else
                app.MainWindow.DevToolsEnabled = false;
                app.MainWindow.ContextMenuEnabled = false;
            #endif


            AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
            {
                app.MainWindow.ShowMessage("Fatal exception", error.ExceptionObject.ToString());
            };

            //app.UseStaticFiles();

            //app.UseRouting();

            //app.MapBlazorHub();
            //app.MapFallbackToPage("/_Host");

            GlobalStopwatch.Start();
            GlobalTimer.StartGlobalTimer();
            
            app.Run();

        }


    }
}