using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace dOSC.Middlewear;

public static class PacketReader
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static T? ReadPacket<T>(this IEnumerable<byte> bytes) where T : class
    {
        var message = Encoding.UTF8.GetString(bytes.ToArray());
        if (typeof(T) == typeof(string))
        {
            object temp = message;
            return temp as T;
        }

        return JsonSerializer.Deserialize<T>(message, _jsonOptions);
    }

    public static ArraySegment<byte> WritePacket(this object command)
    {
        var data = JsonSerializer.Serialize(command, _jsonOptions);
        var buffer = Encoding.UTF8.GetBytes(data);
        return new ArraySegment<byte>(buffer);
    }
}