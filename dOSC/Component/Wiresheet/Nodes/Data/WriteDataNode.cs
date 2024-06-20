using dOSC.Drivers.Hub;
using LiteDB;
using LiveSheet;
using LiveSheet.Parts;
using LiveSheet.Parts.Nodes;
using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Data;

public class WriteDataNode : DataNode
{
    public WriteDataNode() : base()
    {
    }
    
    public override string NodeName => "Write Data";
    public override string Icon => "fa-solid fa-arrow-right-to-bracket";
    public override void Process(List<EffectedNode>? effectedNodes = null)
    {

        var inA = Ports[0];
        if (inA is LivePort a && this.OkToProcess(effectedNodes))
        {
            try
            {
                BsonValue value = BsonValue.Null;
                
                if (a is LiveLogicPort logic)
                {
                    value = a.HasLinks() ? a.GetBsonValue()  : new(false);

                }
                else if (a is LiveNumericPort numeric)
                {
                    value = a.HasLinks() ? a.GetBsonValue() : new((decimal)0.0);
                }
                else if (a is LiveStringPort text)
                {
                    value = a.HasLinks() ? a.GetBsonValue() : new(string.Empty);
                }
                else if (a is LiveTimePort time)
                {
                    value = a.HasLinks() ? a.GetBsonValue() : new BsonValue(new LiveSheetTime(DateTime.MinValue));
                }

                if (value != BsonValue.Null)
                {
                    DataWriterService.NotifyOwner(EndPoint,value);
                    ClearErrorMessage();
                }
            }
            catch
            {
                SetErrorMessage(LiveErrorMessages.FailedToCalculate);
            }
            
        }
        else
        {
            ClearErrorMessage();
        }
    }
}