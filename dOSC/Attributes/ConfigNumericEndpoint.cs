using dOSC.Client.Models.Commands;

namespace dOSC.Attributes;

public class ConfigNumericEndpoint : ConfigEndpoint
{
    public override DataType DataType => DataType.Numeric;
    public double DefaultValue { get;  set; } = 0;
    public double MinValue { get; set; } = SafeDecimalToDouble(decimal.MinValue);
    public double MaxValue { get; set; } = SafeDecimalToDouble(decimal.MaxValue);
    public int Precision { get; set; } = 5;
    public Permissions Permissions { get; set; } 
    public string Unit { get; set; } = "";
    
    
    private static double SafeDecimalToDouble(decimal value)
    {
        if (value < decimal.MinValue) return double.MinValue;
        if (value > decimal.MaxValue) return double.MaxValue;
        return Convert.ToDouble(value);
    }
    
    public decimal SafeDoubleToDecimal(double value)
    {
        if (value < (double)decimal.MinValue) return decimal.MinValue;
        if (value > (double)decimal.MaxValue) return decimal.MaxValue;
        try
        {
            return Convert.ToDecimal(value);
        }
        catch (OverflowException)
        {
            if (value < 0)
                return decimal.MinValue;
            else
                return decimal.MaxValue;
        }
    }
}