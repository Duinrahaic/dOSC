namespace dOSC.Component.Wiresheet.Nodes.Logic;

internal class LogicOperations
{
    /// <summary>
    /// Performs the logical AND operation.
    /// </summary>
    /// <param name="a">First boolean operand.</param>
    /// <param name="b">Second boolean operand.</param>
    /// <returns>True if both operands are true; otherwise, false.</returns>
    public static bool AndOperation(bool a, bool b)
    {
        return a && b;
    }

    /// <summary>
    /// Performs the logical OR operation.
    /// </summary>
    /// <param name="a">First boolean operand.</param>
    /// <param name="b">Second boolean operand.</param>
    /// <returns>True if at least one operand is true; otherwise, false.</returns>
    public static bool OrOperation(bool a, bool b)
    {
        return a || b;
    }

    /// <summary>
    /// Performs the logical NOT operation.
    /// </summary>
    /// <param name="a">Boolean operand.</param>
    /// <returns>True if the operand is false; otherwise, false.</returns>
    public static bool NotOperation(bool a)
    {
        return !a;
    }

    /// <summary>
    /// Performs the logical XOR operation.
    /// </summary>
    /// <param name="a">First boolean operand.</param>
    /// <param name="b">Second boolean operand.</param>
    /// <returns>True if exactly one operand is true; otherwise, false.</returns>
    public static bool XorOperation(bool a, bool b)
    {
        return a ^ b;
    }

    /// <summary>
    /// Performs the logical NAND operation.
    /// </summary>
    /// <param name="a">First boolean operand.</param>
    /// <param name="b">Second boolean operand.</param>
    /// <returns>True if at least one operand is false; otherwise, false.</returns>
    public static bool NandOperation(bool a, bool b)
    {
        return !(a && b);
    }

    /// <summary>
    /// Performs the logical NOR operation.
    /// </summary>
    /// <param name="a">First boolean operand.</param>
    /// <param name="b">Second boolean operand.</param>
    /// <returns>True if both operands are false; otherwise, false.</returns>
    public static bool NorOperation(bool a, bool b)
    {
        return !(a || b);
    }

    /// <summary>
    /// Performs the logical XNOR operation.
    /// </summary>
    /// <param name="a">First boolean operand.</param>
    /// <param name="b">Second boolean operand.</param>
    /// <returns>True if both operands are the same; otherwise, false.</returns>
    public static bool XnorOperation(bool a, bool b)
    {
        return !(a ^ b);
    }
    
       // Comparison Operations

    /// <summary>
    /// Compares if the first value is greater than the second value.
    /// </summary>
    /// <param name="a">First integer value.</param>
    /// <param name="b">Second integer value.</param>
    /// <returns>True if the first value is greater; otherwise, false.</returns>
    public static bool GreaterThan(int a, int b)
    {
        return a > b;
    }

    /// <summary>
    /// Compares if the first value is less than the second value.
    /// </summary>
    /// <param name="a">First integer value.</param>
    /// <param name="b">Second integer value.</param>
    /// <returns>True if the first value is less; otherwise, false.</returns>
    public static bool LessThan(int a, int b)
    {
        return a < b;
    }

    /// <summary>
    /// Compares if the first value is equal to the second value.
    /// </summary>
    /// <param name="a">First integer value.</param>
    /// <param name="b">Second integer value.</param>
    /// <returns>True if both values are equal; otherwise, false.</returns>
    public static bool EqualTo(int a, int b)
    {
        return a == b;
    }

    /// <summary>
    /// Compares if the first value is not equal to the second value.
    /// </summary>
    /// <param name="a">First integer value.</param>
    /// <param name="b">Second integer value.</param>
    /// <returns>True if the values are not equal; otherwise, false.</returns>
    public static bool NotEqualTo(int a, int b)
    {
        return a != b;
    }

    /// <summary>
    /// Compares if the first value is greater than or equal to the second value.
    /// </summary>
    /// <param name="a">First integer value.</param>
    /// <param name="b">Second integer value.</param>
    /// <returns>True if the first value is greater than or equal; otherwise, false.</returns>
    public static bool GreaterThanOrEqualTo(int a, int b)
    {
        return a >= b;
    }

    /// <summary>
    /// Compares if the first value is less than or equal to the second value.
    /// </summary>
    /// <param name="a">First integer value.</param>
    /// <param name="b">Second integer value.</param>
    /// <returns>True if the first value is less than or equal; otherwise, false.</returns>
    public static bool LessThanOrEqualTo(int a, int b)
    {
        return a <= b;
    }
}