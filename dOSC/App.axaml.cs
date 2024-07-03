using System.Runtime.ExceptionServices;
using System.Windows.Forms;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
 
using dOSC.Tray;
using dOSC.Client.ViewModels;
using dOSC.Client.Views;
using dOSC.Drivers;
using dOSC.Drivers.OSC;
using dOSC.Drivers.Pulsoid;
using dOSC.Drivers.Websocket;
using dOSC.Client.Models.Commands;
using dOSC.Tray;
using Application = Avalonia.Application;
using MessageBox = System.Windows.Forms.MessageBox;
using dOSC.Drivers.Hub;

namespace dOSC;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }
  

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if(ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Startup += OnStartup;
            desktop.Exit += Exit;
            desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;
            desktop.MainWindow = new ClientWindow()
            {
                DataContext = new ClientWindowViewModel(),
            };
            
            
        }
        base.OnFrameworkInitializationCompleted();
    }
    
    private void OnStartup(object sender, ControlledApplicationLifetimeStartupEventArgs e)
    {
        TrayIcons? trayIcons = Resources["AppTrayIcon"] as TrayIcons;
        
        if(trayIcons == null)
            return;

        var trayIcon = trayIcons.FirstOrDefault();
        if(trayIcon == null)
            return;
        var menu = trayIcon.Menu;
        if(menu == null)
            return;
   
        
         
        
        var openWindowMenuItem = new NativeMenuItem("Open Client");
        openWindowMenuItem.Click += OpenClientWindow;
        menu.Add(openWindowMenuItem);
        
        menu.Add(new NativeMenuItem("-"));

        var exitMenuItem = new NativeMenuItem("Exit");
        exitMenuItem.Click += Exit;
        menu.Add(exitMenuItem);

        
    }

    private void Exit(object? sender, EventArgs e)
    {
        // Exit the application
        System.Environment.Exit(0);
    }

    private void OpenClientWindow(object? sender, EventArgs e)
    {
        var anotherWindow = new ClientWindow();
        anotherWindow.Show();
    }

    internal static void RunErrorWindowResponse(string[] args, Func<AppBuilder> buildAvaloniaApp)
    {
        var appBuilder = Host.CreateApplicationBuilder(args);
        

    }
    
    
    
    internal static void RunAvaloniaAppWithHosting(string[] args, Func<AppBuilder> buildAvaloniaApp)
    {
        var appBuilder = Host.CreateApplicationBuilder(args);
        appBuilder.Logging.AddDebug();
        appBuilder.Services.AddWindowsFormsBlazorWebView();
        
#if DEBUG
        appBuilder.Services.AddBlazorWebViewDeveloperTools();
        #endif 
        
        appBuilder.RegisterServices();

        using var myApp = appBuilder.Build();
        AppHost = myApp;

        myApp.Start();

        
        try
        {
            buildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        catch(Exception ex)
        {
            var result = MessageBox.Show( ex.ToString(), "Wiresheet Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);            
            Environment.Exit(0);
            
        }
        finally
        {
            Task.Run(async () => await myApp.StopAsync()).GetAwaiter().GetResult();
        }
    }
    
    
}