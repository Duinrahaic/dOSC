using System.Diagnostics;
using System.Reflection;
using dOSC.Component.Wiresheet;
using dOSC.Drivers.Settings;
using LiveSheet;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace dOSC.Utilities;

public static class AppFileSystem
{
    public static string BaseFolderName = "dOSC";
    public static string LogFolderName = "Logs";
    public static string SettingsFolderName = "Settings";
    public static string WiresheetFolderName = "Wiresheets";

    public static string ExecutingFolder =
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

    public static string UpdateFolder = Path.Combine(ExecutingFolder, BaseFolderName, "Update");

    public static string BaseDataFolder =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), BaseFolderName);

    public static string LogFolder = Path.Combine(BaseDataFolder, LogFolderName);
    public static string SettingsFolder = Path.Combine(BaseDataFolder, SettingsFolderName);
    public static string WiresheetFolder = Path.Combine(BaseDataFolder, WiresheetFolderName);
    public static string DownloadsFolder => GetDownloadFolderPath();

    public static string GetDownloadFolderPath()
    {
        return Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Views\CurrentVersion\Explorer\Shell Folders",
            "{374DE290-123F-4565-9164-39C4925E467B}", string.Empty).ToString();
    }

    public static void CreateFolders()
    {
        if (!Directory.Exists(BaseDataFolder)) Directory.CreateDirectory(BaseDataFolder);
        if (!Directory.Exists(LogFolder)) Directory.CreateDirectory(LogFolder);
        if (!Directory.Exists(SettingsFolder)) Directory.CreateDirectory(SettingsFolder);
        if (!Directory.Exists(WiresheetFolder)) Directory.CreateDirectory(WiresheetFolder);
    }
    
    public static void OpenFolder(string folder)
    {
        Process.Start("explorer.exe",folder);
    }

    public static void SaveSettings(UserSettings settings)
    {
        var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
        File.WriteAllText(Path.Combine(SettingsFolder, "settings.json"), json);
    }

    public static void SaveSetting(SettingBase setting)
    {
        var settings = LoadSettings() ?? new UserSettings();
        switch (setting)
        {
            case PulsoidSetting pulsoidSetting:
                settings.Pulsoid = pulsoidSetting;
                break;
            case OSCSetting oscSetting:
                settings.OSC = oscSetting;
                break;
            case WiresheetSetting wiresheetSetting:
                settings.Wiresheet = wiresheetSetting;
                break;
            case WebsocketSetting websocketSetting:
                settings.Websocket = websocketSetting;
                break;
        }
        SaveSettings(settings);
    }

    public static UserSettings? LoadSettings()
    {
        if (!File.Exists(Path.Combine(SettingsFolder, "settings.json")))
        {
            SaveSettings(new UserSettings());
        }

        var json = File.ReadAllText(Path.Combine(SettingsFolder, "settings.json"));
        return JsonConvert.DeserializeObject<UserSettings>(json);
    }


    // App Logic  

    public static void SaveApp(WiresheetDiagram app)
    {
        var json = app.SerializeLiveSheet();
        File.WriteAllText(Path.Combine(WiresheetFolder, $"app-{app.Guid}.json"), json);
    }

    public static void RemoveApp(Guid appGuid)
    {
        RemoveApp(appGuid.ToString());
    }

    public static void RemoveApp(string appGuid)
    {
        if (File.Exists(Path.Combine(WiresheetFolder, $"app-{appGuid}.json")))
            File.Delete(Path.Combine(WiresheetFolder, $"app-{appGuid}.json"));
    }

    public static List<WiresheetDiagram> LoadApps()
    {
        List<WiresheetDiagram> s = new();
        // Get all files in the wiresheet folder
        var files = Directory.GetFiles(WiresheetFolder);
        foreach (var f in files)
        {
            var json = File.ReadAllText(f);
            try
            {
                if (LiveSheetHelper.DeserializeLiveSheet(json) is WiresheetDiagram wiresheet)
                {
                    s.Add(wiresheet);
                }
            }
            catch
            {
                // ignore
            }
        }

        return s;
    }

    private static void RemoveWiresheet(Guid AppGuid)
    {
        if (File.Exists(Path.Combine(WiresheetFolder, $"wiresheet-{AppGuid}.json")))
            File.Delete(Path.Combine(WiresheetFolder, $"wiresheet-{AppGuid}.json"));
    }
}