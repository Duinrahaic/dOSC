using LiteDB;

namespace dOSC.Drivers.Hub;

public static class DataEventManager
{
    // Delegates for the events
    public delegate void DataUpdatedEventHandler(Endpoint endpoint);

    // Events based on the delegates
    public static event DataUpdatedEventHandler? DataUpdated;

    private static Func<Endpoint, Task<BsonValue>>? _dataRetrievalMethod;
    private static Func<Endpoint, BsonValue, Task<bool>>? _dataWriteMethod;

    public static void SetDataRetrievalMethod(Func<Endpoint, Task<BsonValue>> dataRetrievalMethod)
    {
        _dataRetrievalMethod = dataRetrievalMethod ?? throw new ArgumentNullException(nameof(dataRetrievalMethod));
    }

    public static void SetDataWriteMethod(Func<Endpoint, BsonValue, Task<bool>> dataWriteMethod)
    {
        _dataWriteMethod = dataWriteMethod ?? throw new ArgumentNullException(nameof(dataWriteMethod));
    }

    // Method to raise data updated event
    public static void RaiseDataUpdated(Endpoint endpoint)
    {
        // Raise the event if there are any subscribers
        DataUpdated?.Invoke(endpoint);
    }

    // Method to raise data write request event
    public static async Task<bool> RaiseDataWriteRequestAsync(Endpoint endpoint, BsonValue data)
    {
        if (_dataWriteMethod == null)
        {
            throw new InvalidOperationException("Data write method has not been set.");
        }
        return await _dataWriteMethod(endpoint, data);
    }

    public static async Task<BsonValue> RetrieveDataAsync(Endpoint endpoint)
    {
        if (_dataRetrievalMethod == null)
        {
            throw new InvalidOperationException("Data retrieval method has not been set.");
        }
        return await _dataRetrievalMethod(endpoint);
    }
}