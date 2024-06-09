using dOSC.Client.Models.Commands;

namespace dOSC.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ConfigEndpoint : Attribute
{
    public string Owner { get; set; }
    public string Name { get; set; }
    public string Alias { get; set; }
    public string Description { get; set; }
    public virtual DataType DataType { get; private set; }
    public Permissions Permissions { get; set; } 
}