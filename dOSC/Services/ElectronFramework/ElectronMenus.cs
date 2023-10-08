using ElectronNET.API;
using ElectronNET.API.Entities;

namespace dOSC.Services.ElectronFramework
{
    internal static partial class ElectronFramework
    {
        public static MenuItem[] Menus = new MenuItem[]
        {
            new MenuItem {
                Label = "View", Submenu = new MenuItem[] {
                new MenuItem
                {
                    Label = "Reload",
                    Accelerator = "CmdOrCtrl+R",
                    Click = () =>
                    {
                        // on reload, start fresh and close any old
                        // open secondary windows
                        Electron.WindowManager.BrowserWindows.ToList().ForEach(browserWindow => {
                            if(browserWindow.Id != 1)
                            {
                                browserWindow.Close();
                            }
                            else
                            {
                                browserWindow.Reload();
                            }
                        });
                    }
                },
                new MenuItem
                {
                    Label = "Toggle Full Screen",
                    Accelerator = "CmdOrCtrl+F",
                    Click = async () =>
                    {
                        bool isFullScreen = await Electron.WindowManager.BrowserWindows.First().IsFullScreenAsync();
                        Electron.WindowManager.BrowserWindows.First().SetFullScreen(!isFullScreen);
                    }
                },
                new MenuItem
                {
                    Label = "Open Developer Tools",
                    Accelerator = "CmdOrCtrl+I",
                    Click = () => Electron.WindowManager.BrowserWindows.First().WebContents.OpenDevTools()
                },
                new MenuItem
                {
                    Type = MenuType.separator
                },
                new MenuItem
                {
                    Label = "App Menu Demo",
                    Click = async () => {
                        var options = new MessageBoxOptions("This demo is for the Menu section, showing how to create a clickable menu item in the application menu.");
                        options.Type = MessageBoxType.info;
                        options.Title = "Application Menu Demo";
                        await Electron.Dialog.ShowMessageBoxAsync(options);
                    }
                }
                }
            }
        };
    }
}
