using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using dOSC.Shared.Models.Commands;

namespace dOSC.Shared.Models.Websocket.Converters;

public class LabelConverter : JsonConverter<DataLabels>
{
    public override DataLabels? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var jsonObject = doc.RootElement;
            string type = jsonObject.GetProperty("Type").GetString();

            switch (type)
            {

                case "Logic":
                    return JsonSerializer.Deserialize<LogicDataLabels>(jsonObject.GetRawText(), options);
                case "State":
                    return JsonSerializer.Deserialize<StateDataLabels>(jsonObject.GetRawText(), options);
                case "Numeric":
                    return JsonSerializer.Deserialize<NumericDataLabels>(jsonObject.GetRawText(), options);
                default:
                    return null;
            }
        }
    }
    
    public override void Write(Utf8JsonWriter writer, DataLabels value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, options);
    }
}