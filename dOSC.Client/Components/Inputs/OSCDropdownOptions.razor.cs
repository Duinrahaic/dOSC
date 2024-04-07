using Microsoft.AspNetCore.Components;

namespace dOSC.Client.Components.Inputs;

public class DropdownOptions
{
    [Parameter] public string Selection { get; set; } = string.Empty;

    [Parameter] public List<string> Options { get; set; }
}