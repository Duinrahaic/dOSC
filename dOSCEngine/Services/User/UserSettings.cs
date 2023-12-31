﻿using Newtonsoft.Json;

namespace dOSCEngine.Services.User
{
    [JsonObject]
    public class UserSettings
    {
        public dOSCSetting dOSC { get; set; } = new();
        public OSCSetting OSC { get; set; } = new();
        public PulsoidSetting Pulsoid { get; set; } = new();
        public XSOverlaySetting XSOverlay { get; set; } = new();
        public List<SettingBase> GetSettings()
        {
            return new() { dOSC, OSC, Pulsoid, XSOverlay };
        }
    }
}
