using dOSC.Shared.Units;
using dOSC.Shared.Utilities;
using LiveSheet.Parts.Ports;
using LiveSheet.Parts.Serialization;

namespace dOSC.Component.Wiresheet.Nodes.Utility;

public class UtilityDelayNode : UtilityNode
{
    private readonly QueueProcessor<DelayAction> _queue;

    [LiveSerialize] public long DelayTime { get; set; } = 1;
    [LiveSerialize] public TimeUnit TimeUnit { get; set; } = TimeUnit.Second;
    [LiveSerialize] private bool ShowNumbersOnly { get; set; } = false;
    [LiveSerialize] private bool ShowPercent { get; set; } = false;
    [LiveSerialize] public int QueueSize { get; set; } = 1;
    public int ItemsInQueue { get; private set; } = 0;

    private DelayAction? _activeAction = null;

    
    public UtilityDelayNode() : base()
    {
        this.AddPort(new LiveLogicPort(this, true));
        this.AddPort(new LiveLogicPort(this, false));
        
        _queue = new QueueProcessor<DelayAction>(
            Process,
            isSequential: true);
        _queue.StartProcessing();
    }
    
    private async Task Process(DelayAction item)
    {
        _activeAction = item;
        await _activeAction.Start();
        do
        {
            //Progress = _activeAction.CalculateRemainingPercent();
            //VisualIndicator = ActiveAction.IndicatorToString(_showPercent, _showNumbersOnly);
            if (QueueSize > 1) ItemsInQueue = _queue.GetQueueCount();


            await Task.Delay(100);
        } while (_activeAction.CalculateRemainingPercent() != 0);

        //VisualIndicator = ActiveAction.IndicatorToString();
        //Progress = ActiveAction.CalculateRemainingPercent();


        //if (GetCurrentMultiPortType() == PortType.Multi)
        //{
        //    Value = BsonValue.Null;
        //    Queue.ClearQueue();
        //}
        //else
        //{
        //    Value = _activeAction.Value;
        //}
    }
    

    public override void Dispose()
    {
        
        base.Dispose();
    }
}

