using dOSC.Services;
using dOSC.Services.User;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace dOSC.Utilities
{
    public static class FileSystem
    {
        public static string BaseFolderName = "dOSC";
        public static string LogFolderName = "Logs";
        public static string SettingsFolderName = "Settings";
        public static string WiresheetFolderName = "Wiresheets";

        public static string ExecutingFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)??string.Empty;
        public static string BaseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), BaseFolderName);
        public static string LogFolder = Path.Combine(BaseFolder, LogFolderName);
        public static string SettingsFolder = Path.Combine(BaseFolder, SettingsFolderName);
        public static string WiresheetFolder = Path.Combine(BaseFolder, WiresheetFolderName);

        public static void CreateFolders()
        {
            if (!Directory.Exists(BaseFolder))
            {
                Directory.CreateDirectory(BaseFolder);
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
            string json = JsonConvert.SerializeObject(settings,Formatting.Indented);
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
            }
            SaveSettings(settings);
        } 

        public static UserSettings? LoadSettings()
        {
            // if file does not exist make one
            if (!File.Exists(Path.Combine(SettingsFolder, "settings.json"))){
                SaveSettings(new UserSettings());
            }
            string json = File.ReadAllText(Path.Combine(SettingsFolder, "settings.json"));
            return JsonConvert.DeserializeObject<UserSettings>(json);
        }
        public static void RemoveWiresheet(Guid AppGuid)
        {
            if(File.Exists(Path.Combine(WiresheetFolder, $"wiresheet-{AppGuid}.json"))){
                File.Delete(Path.Combine(WiresheetFolder, $"wiresheet-{AppGuid}.json"));
            }
        }
        public static void RemoveWiresheet(dOSCWiresheet wiresheet)
        {
            if (File.Exists(Path.Combine(WiresheetFolder, $"wiresheet-{wiresheet.AppGuid}.json")))
            {
                File.Delete(Path.Combine(WiresheetFolder, $"wiresheet-{wiresheet.AppGuid}.json"));
            }
        }
        public static void SaveWiresheet(dOSCWiresheet wiresheet)
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true,
            };
            string json = JsonConvert.SerializeObject(wiresheet.GetDTO(), Formatting.Indented);
            File.WriteAllText(Path.Combine(WiresheetFolder, $"wiresheet-{wiresheet.AppGuid}.json"), json);
        }
        public static dOSCWiresheetDTO? LoadWiresheet(Guid AppGuid)
        {
            var options = new JsonSerializerSettings
            {

            };
            string json = File.ReadAllText(Path.Combine(WiresheetFolder, $"wiresheet-{AppGuid}.json"));
            return JsonConvert.DeserializeObject<dOSCWiresheetDTO>(json, options);
        }
        public static List<dOSCWiresheetDTO> LoadWiresheets()
        {
            var options = new JsonSerializerSettings
            {

            };
            List<dOSCWiresheetDTO> s = new();
            // Get all files in the wiresheet folder
            string[] files = Directory.GetFiles(WiresheetFolder);
            foreach(var f in files)
            {
                string json = File.ReadAllText(f);
                var obj = JsonConvert.DeserializeObject<dOSCWiresheetDTO>(json, options);
                if(obj != null)
                {
                    s.Add(obj);
                }
            }
            return s;
        }

        public async static Task DownloadWiresheet(IJSRuntime js, dOSCWiresheet wiresheet )
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true,
            };
            string json = JsonConvert.SerializeObject(wiresheet.GetDTO(), Formatting.Indented);
            string filename = $"wiresheet-{wiresheet.AppGuid}.json";
            byte[] data = Encoding.UTF8.GetBytes(json);
            await js.InvokeAsync<object>(
                "saveAsFile",
                filename,
                Convert.ToBase64String(data));
        }
    }
}
