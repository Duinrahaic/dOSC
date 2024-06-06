using dOSC.Component.Wiresheet.Shared;
using LiveSheet;
using LiveSheet.Parts.Ports;
using LiveSheet.Parts.Serialization;

namespace dOSC.Component.Wiresheet.Nodes.Variables;

public class VariableTimeNode: VariableNode
{
    public VariableTimeNode() : base()
    {
        SilentSetValue(new LiveSheetTime(DateTime.Now));
        AddPort(new LiveTimePort(this, false));
    }

    public override string NodeName => "Time Variable";
    public override string Icon => "fa-solid fa-clock";
    
    
    [LiveSerialize]
    public TimeOptions Option { get; set; } = TimeOptions.DateTime;
    
    
    public override string GetDisplayValue()
    {
        if (LiveSheetTime.IsLiveSheetTime(Value) )
        {
            DateTime time = (LiveSheetTime)Value;
            
            if(Option == TimeOptions.DateTime)
                return time.ToString("g", System.Globalization.CultureInfo.CurrentCulture);
            if(Option == TimeOptions.DateOnly)
                return time.ToString("d", System.Globalization.CultureInfo.CurrentCulture);
            if(Option == TimeOptions.TimeOnly)
                return time.ToString("t", System.Globalization.CultureInfo.CurrentCulture);
        }
        return "Invalid Time Value";
    }
    
    public DateTime GetDateTimeValue()
    {
        if (LiveSheetTime.IsLiveSheetTime(Value))
        {
            return (LiveSheetTime)Value;
        }
        return DateTime.Now;
    }
    
    
    public void SetDateTimeValue(DateTime time)
    {
        Value = new LiveSheetTime(time);
    }
}