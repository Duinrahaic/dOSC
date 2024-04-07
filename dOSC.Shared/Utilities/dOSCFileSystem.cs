﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using dOSC.Shared.Models.Settings;
using dOSC.Shared.Models.Wiresheet;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace dOSC.Shared.Utilities;

public static class dOSCFileSystem
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

    public static void SaveSettings(UserSettings settings)
    {
        var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
        File.WriteAllText(Path.Combine(SettingsFolder, "settings.json"), json);
    }

    public static void SaveSetting(SettingBase setting)
    {
        var settings = LoadSettings() ?? new UserSettings();
        switch (setting.SettingType)
        {
            case SettingType.Pulsoid:
                settings.Pulsoid = (PulsoidSetting)setting;
                break;
            case SettingType.OSC:
                settings.OSC = (OSCSetting)setting;
                break;
            case SettingType.dOSC:
                settings.dOSC = (dOSCSetting)setting;
                break;
        }

        SaveSettings(settings);
    }

    public static UserSettings? LoadSettings()
    {
        // if file does not exist make one
        if (!File.Exists(Path.Combine(SettingsFolder, "settings.json"))) SaveSettings(new UserSettings());
        var json = File.ReadAllText(Path.Combine(SettingsFolder, "settings.json"));
        return JsonConvert.DeserializeObject<UserSettings>(json);
    }


    // App Logic  

    public static void SaveApp(dOSCDataDTO app)
    {
        var json = JsonConvert.SerializeObject(app, Formatting.Indented);
        File.WriteAllText(Path.Combine(WiresheetFolder, $"app-{app.AppGuid}.json"), json);
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

    public static List<dOSCDataDTO> LoadApps()
    {
        List<dOSCDataDTO> s = new();
        // Get all files in the wiresheet folder
        var files = Directory.GetFiles(WiresheetFolder);
        foreach (var f in files)
        {
            var json = File.ReadAllText(f);
            try
            {
                var obj = JsonConvert.DeserializeObject<dOSCDataDTO>(json);
                if (obj != null)
                {
                    s.Add(obj);
                    if (f.Contains("wiresheet"))
                    {
                        SaveApp(obj);
                        RemoveWiresheet(obj.AppGuid);
                    }
                }
            }
            catch
            {
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