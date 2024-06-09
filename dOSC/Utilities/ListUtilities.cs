using System;
using System.Collections.Generic;

namespace dOSC.Utilities;

public static class ListUtilities
{
    public static List<T> GetEnumValues<T>()
    {
        if (!typeof(T).IsEnum) throw new ArgumentException("DataType parameter must be an enum");

        return new List<T>(Enum.GetValues(typeof(T)) as IEnumerable<T>);
    }

    public static List<string> GetEnumNames<T>()
    {
        if (!typeof(T).IsEnum) throw new ArgumentException("DataType parameter must be an enum");

        return new List<string>(Enum.GetNames(typeof(T)));
    }
}