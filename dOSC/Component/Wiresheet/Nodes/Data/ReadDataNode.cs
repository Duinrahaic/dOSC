using dOSC.Client.Models.Commands;
using dOSC.Drivers.Hub;
using LiteDB;
using LiveSheet.Parts.Ports;
using LiveSheet.Parts.Serialization;

namespace dOSC.Component.Wiresheet.Nodes.Data;

public class ReadDataNode: DataNode
{
    public ReadDataNode() : base()
    {
        SubscriptionService.SubscriptionUpdated += SubscriptionService_SubscriptionUpdated;
    }

    private void SubscriptionService_SubscriptionUpdated(Subscription subscription, DataEndpoint endpoint)
    {
        SilentUpdateEndpoint(endpoint);
        Value = EndpointToBsonValue(endpoint);
    }


    public override string NodeName => "Read Data";
    public override string Icon => "fa-solid fa-arrow-right-from-bracket";

    public override void Dispose()
    {
        SubscriptionService.SubscriptionUpdated -= SubscriptionService_SubscriptionUpdated;
        base.Dispose();
    }
}