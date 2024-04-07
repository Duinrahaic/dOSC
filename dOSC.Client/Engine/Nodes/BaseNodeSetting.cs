using System.ComponentModel.DataAnnotations;

namespace dOSC.Client.Engine.Nodes;

public class BaseNodeSetting
{
    [StringLength(125)]
    [RegularExpression(@"^[^,:*?""<>\|]*$", ErrorMessage = @"Cannot use illegal characters :*?""<>\|")]
    public string DisplayName { get; set; } = string.Empty;

    public string Name { get; set; } = "Unknown";
}