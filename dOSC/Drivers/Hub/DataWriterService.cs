using dOSC.Client.Models.Commands;
using LiteDB;

namespace dOSC.Drivers.Hub;
public static class DataWriterService
{
    private static readonly Dictionary<string, Action<DataEndpoint, BsonValue>> Owners 
        = new Dictionary<string, Action<DataEndpoint, BsonValue>>();

    public static void RegisterOwner(string ownerId, Action<DataEndpoint, BsonValue> updateHandler)
    {
        Owners.TryAdd(ownerId, updateHandler);
    }

    public static void UnregisterOwner(string ownerId)
    {
        Owners.Remove(ownerId);
    }

    public static void NotifyOwner(DataEndpoint endpoint, BsonValue value)
    {
        if (Owners.TryGetValue(endpoint.Owner, out var updateHandler))
        {
            updateHandler.Invoke(endpoint, value);
        }
    }
}