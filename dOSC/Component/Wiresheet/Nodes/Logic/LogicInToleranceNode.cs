using LiteDB;
using LiveSheet;
using LiveSheet.Parts;
using LiveSheet.Parts.Nodes;
using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Logic;

public class LogicInToleranceNode: LogicNode
{
    public LogicInToleranceNode() : base()
    {
        AddPort(new LiveNumericPort(this, true, name: "Input"));
        AddPort(new LiveNumericPort(this, true, name: "Target"));
        AddPort(new LiveNumericPort(this, true, name: "Tolerance"));
        AddPort(new LiveLogicPort(this, false, name: "Output"));
    }
    public override string NodeName => "In Tolerance";
    public override string Icon => "fa-solid fa-plus-minus";   
    public override void Process(List<EffectedNode>? effectedNodes = null)
    {
        var inInput = Ports[0];
        var inTarget = Ports[1];
        var inTolerance = Ports[2];
        if(inInput is LiveNumericPort input && inTarget is LiveNumericPort target && inTolerance is LiveNumericPort tolerance && this.OkToProcess(effectedNodes))
        {
            try
            {
                BsonValue inputVal = input.HasLinks() ? input.GetBsonValue() : new(0.0);
                BsonValue targetVal = target.HasLinks() ? target.GetBsonValue() : new(0.0);
                BsonValue toleranceVal = tolerance.HasLinks() ? tolerance.GetBsonValue() : new(1.0);

                Value = InTolerance(inputVal, targetVal, toleranceVal);
                ClearErrorMessage();
            }
            catch
            {
                SetErrorMessage(LiveErrorMessages.BadConfiguration);
            }
        }
        else
        {
            Value = false;
            ClearErrorMessage();
        }
    }
    
    private bool InTolerance(double setpoint, double actual, double tolerance = 0.01)
    {
        double error = setpoint - actual;
        return !(Math.Abs(error) > tolerance);
    }
    
}
