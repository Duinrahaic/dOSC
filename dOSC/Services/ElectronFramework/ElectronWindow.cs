using ElectronNET.API.Entities;
using ElectronNET.API;

namespace dOSC.Services.ElectronFramework
{
    internal static partial class ElectronFramework
    {
        public static WebApplicationBuilder? LaunchElectronWindow(this WebApplicationBuilder? builder, string[] args)
        {
            if (builder == null) throw new ArgumentNullException(nameof(Services));
            var options = new BrowserWindowOptions()
            {
                Frame = false,
                Transparent = false,
                TitleBarStyle = TitleBarStyle.customButtonsOnHover,
                DarkTheme = true,
                Focusable = true,
                Resizable = true,
                Movable = true,
                EnableLargerThanScreen = false,
                Center = true,  
                WebPreferences = new()
                {
                    ContextIsolation = true,
                    DevTools = false,
                    WebSecurity = false,
                    EnableRemoteModule = true,

                },
                
            };

            

            builder.WebHost.UseElectron(args);
            builder.ConfigureElectronTheme();
            Electron.Menu.SetApplicationMenu(Menus);
            Electron.IpcMain.On("put-in-tray", (args) => {

                if (Electron.Tray.MenuItems.Count == 0)
                {
                    var menu = new MenuItem
                    {
                        Label = "Remove",
                        Click = () => Electron.Tray.Destroy()
                    };

                    Electron.Tray.Show("/Assets/electron_32x32.png", menu);
                    Electron.Tray.SetToolTip("Electron Demo in the tray.");
                }
                else
                {
                    Electron.Tray.Destroy();
                }

            });
            
            // Open the Electron-Window here
            Task.Run(async () =>
            {
                var window = await Electron.WindowManager.CreateWindowAsync(options);
                
                window.OnClose += () =>
                {
                    Electron.IpcMain.Send(window, "put-in-tray");
                };
                window.OnClosed += () =>
                {
                    
                };

                // Window Crash
                window.WebContents.OnCrashed += async (killed) =>
                {
                    var options = new MessageBoxOptions("This process has crashed.")
                    {
                        Type = MessageBoxType.info,
                        Title = "Renderer Process Crashed",
                        Buttons = new string[] { "Reload", "Close" }
                    };
                    var result = await Electron.Dialog.ShowMessageBoxAsync(options);

                    if (result.Response == 0)
                    {
                        window.Reload();
                    }
                    else
                    {
                        window.Close();
                    }
                };
            });
            return builder;
        }
    }
}
