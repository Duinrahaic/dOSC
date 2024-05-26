using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using dOSC.Client.ViewModels;


namespace dOSC.Tray;

public class TrayIconViewModel: ViewModelBase
{
 
    public TrayIconViewModel()
    {
  
    }

    private void ShowWindow()
    {
 

    }

    private static void Exit()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime application)
        {
            application.Shutdown();
        }
    }
}