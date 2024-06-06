using dOSC.Component.Wiresheet.Shared;
using LiveSheet;
using LiveSheet.Parts.Events;
using LiveSheet.Parts.Ports;
using LiveSheet.Parts.Serialization;
using LiveSheet.Utilities;

namespace dOSC.Component.Wiresheet.Nodes.Time;

public class RealTimeNode : TimeNode
{
    public RealTimeNode() : base()
    {
        SyncedTimer.TimeUpdated += OnTimedEvent;   
        this.AddPort(new LiveTimePort(this, false,name:"Current Time"));
    }
    
    public override string NodeName => "Real-Time Node";
    public override string Icon => "icon-watch";
    
    private void OnTimedEvent(object? sender, TimeEventArgs e)
    {
        Value = e.CurrentTime;
    }
    
    
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
    
    
    public override void Dispose()
    {
        SyncedTimer.TimeUpdated -= OnTimedEvent;
        base.Dispose();
    }
}