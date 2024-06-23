namespace dOSC.Drivers.Settings;

public class VRChatSettings : SettingBase
{
    public string AvatarConfigPath { get; set; } = @"~\AppData\LocalLow\VRChat\VRChat\OSC\";
    public bool AllowAvatarConfigLearning { get; set; } = false;
    
}