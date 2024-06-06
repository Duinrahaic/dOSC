using LiveSheet;
using LiveSheet.Parts.Events;
using LiveSheet.Utilities;

namespace dOSC.Component.Wiresheet.Nodes;

public abstract class TimeNode: WiresheetNode
{
    public TimeNode() : base()
    {
        SilentSetValue(new LiveSheetTime(DateTime.Now));
    }
    public override string NodeName => "Time Node";
    public override NodeCategory Category => NodeCategory.Time;


}