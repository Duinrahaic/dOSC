using Newtonsoft.Json;

namespace dOSC.Client.Services.Connectors.Hub.Activity.Pulsoid;

public class PulsoidData
{
    [JsonProperty("heart_rate")] public int HeartRate { get; set; }
}