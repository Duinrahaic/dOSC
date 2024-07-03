using CoreOSC;
using dOSC.Client.Models.Commands;
using LiteDB;

namespace dOSC.Utilities;

public static class OscExtensions
{
    public static DataEndpoint? GetValueFromOscMessage(this OscMessage message, string owner)
    {
        var arg = message.Arguments.FirstOrDefault();
        
        var ep = new DataEndpoint
        {
            Owner = owner,
            Name = message.Address,
            Alias = message.Address.Split('/').LastOrDefault(),
            System = false,
            Permissions = Permissions.ReadWrite,
        };
        
        if (arg == null)
        {
            return null;
        }

        if (arg is System.Int32)
        {
            ep.Type = DataType.Numeric;
            ep.Constraints = new Constraints
            {
                Min = 0,
                Max = 255,
                Precision = 0
            };
            ep.DefaultValue = "0";
        }
        else if (arg is System.Double || arg is System.Single)
        {
            ep.Type = DataType.Numeric;
            ep.Constraints = new Constraints
            {
                Min = -1,
                Max = 1,
                Precision = 5
            };
            ep.DefaultValue = "0.000";

        }
        else if (arg is System.String)
        {
            ep.Type = DataType.Text;
        }
        else if (arg is System.Boolean)
        {
            ep.Type = DataType.Logic;
            ep.DefaultValue = "false";
        }
        else
        {
            return null;
        }
        return ep;
    }
}