using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using dOSC.Client.Models.Commands;

namespace dOSC.Client.Models.Websocket.Converters;

public class DataConverter : JsonConverter<Data>
{
    public override Data? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (var doc = JsonDocument.ParseValue(ref reader))
        {
            var jsonObject = doc.RootElement;
            var typeString = jsonObject.GetProperty("Type").GetString();
            if (!Enum.TryParse(typeString, out CommandType type)) throw new JsonException($"None type: {typeString}");

            switch (type)
            {
                case CommandType.Log:
                    return JsonSerializer.Deserialize<Log>(jsonObject.GetRawText(), options);
                case CommandType.RegisterEndpoint:
                case CommandType.UnregisterEndpoint:
                case CommandType.UpdateEndpoint:
                    return JsonSerializer.Deserialize<DataEndpoint>(jsonObject.GetRawText(), options);
                default:
                    return null;
            }
        }
    }

    public override void Write(Utf8JsonWriter writer, Data value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, options);
    }
}