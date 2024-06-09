namespace dOSC.Client.Models.Commands;

public class DataEndpointValue
{
    public string Owner { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DataType Type { get; set; } = DataType.Text;
    public string Value { get; set; } = string.Empty;

    public void UpdateValue(string value)
    {
        Value = value;
    }

    public void UpdateValue(bool value)
    {
        Value = value.ToString();
    }

    public void UpdateValue(decimal value)
    {
        Value = value.ToString();
    }
}