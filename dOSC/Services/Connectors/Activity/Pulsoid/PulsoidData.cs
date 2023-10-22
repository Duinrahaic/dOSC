using Newtonsoft.Json;

namespace dOSC.Services.Connectors.Activity.Pulsoid
{
    public class PulsoidData
    {
        [JsonProperty("heart_rate")]
        public int HeartRate { get; set; }
    }
}
