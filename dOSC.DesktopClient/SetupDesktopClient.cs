using Avalonia;
using Avalonia.ReactiveUI;
using Avalonia.WebView.Desktop;

namespace dOSC.Client;

public static class SetupDesktopClient
{
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<dOSC.Client.Desktop.App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI()
            .UseDesktopWebView();
}