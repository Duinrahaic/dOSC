using Newtonsoft.Json;

namespace dOSCEngine.Services.Connectors.Activity.Pulsoid
{
    public class PulsoidData
    {
        [JsonProperty("heart_rate")]
        public int HeartRate { get; set; }
    }
}
