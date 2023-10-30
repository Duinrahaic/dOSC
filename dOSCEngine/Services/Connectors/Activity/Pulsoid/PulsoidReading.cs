using Newtonsoft.Json;
namespace dOSCEngine.Services.Connectors.Activity.Pulsoid
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PulsoidReading : EventArgs
    {
        [JsonProperty("measured_at")]
        public string Timestamp { get; set; }
        [JsonProperty("data")]
        public PulsoidData Data { get; set; } = new PulsoidData();
    }
}
