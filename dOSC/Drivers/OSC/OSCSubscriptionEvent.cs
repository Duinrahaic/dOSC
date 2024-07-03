using System;
using System.Collections.Generic;
using CoreOSC;
using dOSC.Client.Models.Commands;
using dOSC.Utilities;
using LiteDB;

namespace dOSC.Drivers.OSC;

public class OSCSubscriptionEvent : EventArgs
{
    public OSCSubscriptionEvent(OscMessage message)
    {
        Message = message;
    }
    public  OscMessage Message { get; private set; }
    
    public DataEndpoint? GetEndpoint(string owner)
    {
        return Message.GetValueFromOscMessage(owner);
    }
    
    public BsonValue EndpointToBsonValue(DataEndpoint endpoint)
    {
        try
        {
            BsonValue value = BsonValue.Null;
            switch (endpoint.Type)
            {
                case DataType.Logic:
                    value = new BsonValue(Convert.ToBoolean(Message.Arguments[0]));
                    break;
                case DataType.Numeric:
                    value = new BsonValue(Convert.ToDecimal(Message.Arguments[0]));
                    break;
                case DataType.Text:
                    value = new BsonValue(Message.Arguments[0].ToString());
                    break;
            }

            return value;
        }
        catch
        {
            return BsonValue.Null;
        }
        
    }
    
}