using dOSCEngine.Engine;
using dOSCEngine.Services;
using dOSCEngine.Services.User;
using Microsoft.JSInterop;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace dOSCEngine.Utilities
{
    public static class dOSCFileSystem
    {
        public static string BaseFolderName = "dOSC";
        public static string LogFolderName = "Logs";
        public static string SettingsFolderName = "Settings";
        public static string WiresheetFolderName = "Wiresheets";

        public static string ExecutingFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;
        public static string UpdateFolder = Path.Combine(ExecutingFolder, BaseFolderName, "Update");
        public static string BaseDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), BaseFolderName);
        public static string LogFolder = Path.Combine(BaseDataFolder, LogFolderName);
        public static string SettingsFolder = Path.Combine(BaseDataFolder, SettingsFolderName);
        public static string WiresheetFolder = Path.Combine(BaseDataFolder, WiresheetFolderName);
        public static string DownloadsFolder => GetDownloadFolderPath();

        public static string GetDownloadFolderPath()
        {
            return Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Views\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
        }

        public static void CreateFolders()
        {
            if (!Directory.Exists(BaseDataFolder))
            {
                Directory.CreateDirectory(BaseDataFolder);
            }
            if (!Directory.Exists(LogFolder))
            {
                Directory.CreateDirectory(LogFolder);

            }
            if (!Directory.Exists(SettingsFolder))
            {
                Directory.CreateDirectory(SettingsFolder);
            }
            if (!Directory.Exists(WiresheetFolder))
            {
                Directory.CreateDirectory(WiresheetFolder);
            }
        }

        public static void SaveSettings(UserSettings settings)
        {
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
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
            if (!File.Exists(Path.Combine(SettingsFolder, "settings.json")))
            {
                SaveSettings(new UserSettings());
            }
            string json = File.ReadAllText(Path.Combine(SettingsFolder, "settings.json"));
            return JsonConvert.DeserializeObject<UserSettings>(json);
        }

        
        

        public async static Task DownloadWiresheet(IJSRuntime js, dOSCData wiresheet)
        {
			await js.InvokeVoidAsync("GenerateToasterMessage", "Sent app to to downloads folder!");
			string json = JsonConvert.SerializeObject(wiresheet.GetDTO(), Formatting.Indented);
            string filename = $"wiresheet-{wiresheet.AppGuid}.json";
            byte[] data = Encoding.UTF8.GetBytes(json);
            await js.InvokeAsync<object>( "saveAsFile", filename, Convert.ToBase64String(data));
        }



        // App Logic  

        public static void SaveApp(AppLogic app)
        {
            string json = JsonConvert.SerializeObject(app.GetDTO(), Formatting.Indented);
            File.WriteAllText(Path.Combine(WiresheetFolder, $"app-{app.Data.AppGuid}.json"), json);
        }
        private static void SaveApp(dOSCDataDTO app)
        {
            string json = JsonConvert.SerializeObject(app, Formatting.Indented);
            File.WriteAllText(Path.Combine(WiresheetFolder, $"app-{app.AppGuid}.json"), json);
        }
        public static void RemoveApp(AppLogic app) => RemoveApp(app.AppGuid.ToString() ?? string.Empty);
        public static void RemoveApp(dOSCData wiresheet) => RemoveApp(wiresheet.AppGuid);
        public static void RemoveApp(Guid AppGuid) => RemoveApp(AppGuid.ToString());
        public static void RemoveApp(string AppGuid)
        {
            if (File.Exists(Path.Combine(WiresheetFolder, $"app-{AppGuid}.json")))
            {
                File.Delete(Path.Combine(WiresheetFolder, $"app-{AppGuid}.json"));
            }
        }
        public static List<dOSCDataDTO> LoadApps()
        {
            List<dOSCDataDTO> s = new();
            // Get all files in the wiresheet folder
            string[] files = Directory.GetFiles(WiresheetFolder);
            foreach (var f in files)
            {
                string json = File.ReadAllText(f);
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
            {
                File.Delete(Path.Combine(WiresheetFolder, $"wiresheet-{AppGuid}.json"));
            }
        }
        public async static Task DownloadApp(this IJSRuntime js, AppLogic AppLogic)
        {
            await js.InvokeVoidAsync("GenerateToasterMessage", "Sent app to to downloads folder!");
            string json = JsonConvert.SerializeObject(AppLogic.Data.GetDTO(AppLogic.IsEnabled(), AppLogic.IsRunning()), Formatting.Indented);
            string filename = $"app-{AppLogic.AppGuid}.json";
            byte[] data = Encoding.UTF8.GetBytes(json);
            await js.InvokeAsync<object>("saveAsFile", filename, Convert.ToBase64String(data));
        }
    }
}
