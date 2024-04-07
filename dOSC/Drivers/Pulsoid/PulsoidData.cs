using Newtonsoft.Json;

namespace dOSC.Drivers.Pulsoid;

public class PulsoidData
{
    [JsonProperty("heart_rate")] public int HeartRate { get; set; }
}