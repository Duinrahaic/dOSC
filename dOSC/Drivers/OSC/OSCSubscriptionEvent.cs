using System;
using System.Collections.Generic;
using dOSC.Client.Models.Commands;
using LiteDB;

namespace dOSC.Drivers.OSC;

public class OSCSubscriptionEvent : EventArgs
{
    public OSCSubscriptionEvent(string address, List<object> args)
    {
        Address = address;
        Arguments = args;
    }

    public string Address { get; set; }
    public List<object> Arguments { get; set; }

    public BsonValue EndpointToBsonValue(DataEndpoint endpoint)
    {
        try
        {
            BsonValue value = BsonValue.Null;
            switch (endpoint.Type)
            {
                case DataType.Logic:
                    value = new BsonValue(Convert.ToBoolean(Arguments[0]));
                    break;
                case DataType.Numeric:
                    value = new BsonValue(Convert.ToDecimal(Arguments[0]));
                    break;
                case DataType.Text:
                    value = new BsonValue(Arguments[0].ToString());
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