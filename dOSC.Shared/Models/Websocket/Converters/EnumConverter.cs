using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace dOSC.Shared.Models.Websocket.Converters;

public class EnumConverter<T> : JsonConverter<T> where T : struct, Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string enumString = reader.GetString();
        if (Enum.TryParse(enumString, true, out T value))
        {
            return value;
        }
        return default;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
    
}
