using System;

namespace dOSC.Shared.Utilities;

public static class Classifier
{
    public static bool IsNumericType(Type type)
    {
        return type == typeof(byte) || type == typeof(sbyte) || type == typeof(short) || type == typeof(ushort) ||
               type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) ||
               type == typeof(float) || type == typeof(double) || type == typeof(decimal) ||
               type == typeof(short) || type == typeof(int) || type == typeof(long);
    }

    public static bool IsBooleanType(Type type)
    {
        return type == typeof(bool) || type == typeof(sbyte);
    }
}