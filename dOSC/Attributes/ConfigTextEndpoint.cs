using dOSC.Client.Models.Commands;

namespace dOSC.Attributes;

public class ConfigTextEndpoint: ConfigEndpoint
{
    public override DataType DataType => DataType.Text;
    public string DefaultValue { get;  set; } = string.Empty;
    public int MinValue { get; set; } = 0;
    public int MaxValue { get; set; } = 255;
    public List<string> IllegalCharacters { get; set; } = new List<string>();
}