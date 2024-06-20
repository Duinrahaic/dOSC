using dOSC.Client.Models.Commands;

namespace dOSC.Attributes;

public class ConfigLogicEndpoint : ConfigEndpoint
{
    public override DataType DataType => DataType.Logic; 
    public bool DefaultValue { get; set; } = false;
    public string TrueLabel { get; set; } = "True";
    public string FalseLabel { get; set; } = "False";
}