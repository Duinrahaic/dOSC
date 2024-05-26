using dOSC.Shared.Utilities;
using LiveSheet;
using LiveSheet.Parts.Serialization;

namespace dOSC.Component.Wiresheet;

public class WiresheetDiagram: LiveSheetDiagram
{
    [LiveSerialize] public string AppIcon { get; set; } = string.Empty;
    [LiveSerialize] public string Author { get; set; } = string.Empty;
    [LiveSerialize] public string Description { get; set; } = string.Empty;
    [LiveSerialize] public string Repository { get; set; } = string.Empty;
    
    
    public string GetCurrentIcon()
    {
        if (!string.IsNullOrEmpty(AppDefaults.GetDefaultAppImage()))
            return AppIcon;
        return AppDefaults.GetDefaultAppImage();
    }
}