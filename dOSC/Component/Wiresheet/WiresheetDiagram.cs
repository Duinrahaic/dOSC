using System.ComponentModel.DataAnnotations;
using dOSC.Utilities;
using LiveSheet;
using LiveSheet.Parts.Serialization;

namespace dOSC.Component.Wiresheet;

public class WiresheetDiagram() : LiveSheetDiagram(DefaultOptions)
{
    [LiveSerialize] 
    public string AppIcon { get; set; } = string.Empty;
    [LiveSerialize] 
    public string Author { get; set; } = string.Empty;
    [LiveSerialize] 
    public string Description { get; set; } = string.Empty;
    [LiveSerialize] 
    public string Repository { get; set; } = string.Empty;
    [LiveSerialize] 
    [MinLength(6, ErrorMessage = "Application name must be at least 6 characters.")]
    public string Name { get; set; } = string.Empty;
    public string DefaultName => "New App";
    
    public string GetDisplayName()
    {
        if (!string.IsNullOrEmpty(Name))
            return Name;
        return DefaultName;
    }

    public string GetCurrentIcon()
    {
        return !string.IsNullOrEmpty(AppIcon) ? AppIcon : AppDefaults.GetDefaultAppImage();
    }

    public string Serialize()
    {
        return this.SerializeLiveSheet();
    }

    public static WiresheetDiagram Deserialize(string json)
    {
        return new();
    }


}