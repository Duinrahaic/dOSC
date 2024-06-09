using System;

namespace dOSC.Client.Models.Commands;

public class StateDataLabels : DataLabels
{
    public string[] Labels { get; set; } = Array.Empty<string>();
}