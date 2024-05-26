using System;
using Newtonsoft.Json;

namespace dOSC.Drivers.Pulsoid;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class PulsoidReading : EventArgs
{
    [JsonProperty("measured_at")] public string Timestamp { get; set; }

    [JsonProperty("data")] public PulsoidData Data { get; set; } = new();
}