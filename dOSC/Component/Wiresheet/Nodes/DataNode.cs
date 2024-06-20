using System.Globalization;
using dOSC.Client.Models.Commands;
using dOSC.Component.Wiresheet.Nodes.Data;
using dOSC.Drivers.Hub;
using DynamicData;
using LiteDB;
using LiveSheet;
using LiveSheet.Parts.Ports;
using LiveSheet.Parts.Serialization;

namespace dOSC.Component.Wiresheet.Nodes;

public abstract class DataNode: WiresheetNode
{
    public delegate void EndpointChanged(DataEndpoint newEndpoint);
    public event EndpointChanged? OnEndpointChanged;
    
    public DataNode() : base()
    {
        this.SilentSetValue(0);
    }
    
    public override string NodeName => "Data Node";
    public override NodeCategory Category => NodeCategory.Data;
    
    private DataEndpoint _endpoint = new();
    
    internal void SilentUpdateEndpoint(DataEndpoint endpoint)
    {
        if(endpoint.Owner != _endpoint.Owner || endpoint.Name != _endpoint.Name)
        {
            return;
        }
        _endpoint = endpoint;
        UpdatePort();
    }

    public override string GetDisplayValue()
    {
        
        if(EndPoint.Type == DataType.Unknown)
        {
            return Value.ToString();
        }
        if (LiveSheetTime.IsLiveSheetTime(Value.ToString()))
        {
            DateTime time = (LiveSheetTime)Value;
            return time.ToString("g", CultureInfo.CurrentCulture);
        }
        if(EndPoint.Labels is NumericDataLabels numLabels)
        {
            decimal value = Value.AsDecimal;
            return $"{value.ToString($"F{EndPoint.Constraints.Precision}")} {numLabels.Unit}";
        }
        else if(EndPoint.Labels is LogicDataLabels logicLabels)
        {
            if(Value.AsBoolean)
            {
                return logicLabels.TrueLabel;
            }
            else
            {
                return logicLabels.FalseLabel;
            }
        }
        else
        {
            return Value.ToString();
        }
    }

    [LiveSerialize] public DataEndpoint EndPoint { 
        get => _endpoint;
        set
        {
            if(value != null)
            {
                SetEndpoint(value);
            }
        }
    }
    
    protected void SetEndpoint(DataEndpoint endpoint)
    {
        SubscriptionService.Unsubscribe(this.Guid, _endpoint.Owner, _endpoint.Name);
        _endpoint = endpoint;
        UpdatePort();
        if (!string.IsNullOrEmpty(_endpoint.Name) || !string.IsNullOrEmpty(_endpoint.Owner))
        {
            _endpoint = SubscriptionService.Subscribe(this.Guid, _endpoint.Owner, _endpoint.Name);
            SilentSetValue(EndpointToBsonValue(_endpoint));
        }
        OnEndpointChanged?.Invoke(endpoint);
    }

    private bool GetPortDirection()
    {
        if (this is ReadDataNode read)
        {
            return false;
        }
        return true;
    }
    
    protected virtual void UpdatePort()
    {
        bool updatePort = false;
        var portType = EndPoint.Type switch
        {
            DataType.Logic => PortType.Logic,
            DataType.Numeric => PortType.Numeric,
            DataType.Text => PortType.String,
            DataType.Time => PortType.Time,
            _ => PortType.None
        };
        LivePort? currentPort = null;
        if (Ports.Any())
        {
            currentPort = Ports.First() as LivePort;
        }
        
        if(currentPort != null) 
        {
            if (currentPort.PortType != portType)
            {
                updatePort = true;
            }
        }
        else
        {
            updatePort = true;
        }
        

        if (updatePort)
        {
            if (currentPort != null)
            {
                var link = currentPort.GetFirstLink();
                if (link != null)
                {
                    link.Diagram.Links.Remove(link);
                }
                RemovePort(currentPort);
            }
            LivePort newPort;
            switch (portType)
            {
                case PortType.Logic:
                    newPort = new LiveLogicPort(this, GetPortDirection(), name: "");
                    break;
                case PortType.Numeric:
                    newPort = new LiveNumericPort(this, GetPortDirection(), name: "");
                    break;
                case PortType.String:
                    newPort = new LiveStringPort(this, GetPortDirection(), name: "");
                    break;
                case PortType.Time:
                    newPort = new LiveTimePort(this, GetPortDirection(), name: "");
                    break;
                default:
                    newPort = null;
                    break;
            }
            if (newPort != null)
            {
                AddPort(newPort);
            }
            ReinitializePorts();
        }
    }
    internal static BsonValue EndpointToBsonValue(DataEndpoint e)
    {
        BsonValue value = BsonValue.Null;
        switch (e.Type)
        {
            case DataType.Text:
                value = e.DefaultValue;
                break;
            case DataType.Numeric:
                if (decimal.TryParse(e.DefaultValue, out decimal num))
                {
                    value = num;
                }
                break;
            case DataType.Logic:
                if (bool.TryParse(e.DefaultValue, out bool logic))
                {
                    value = logic;
                }
                break;
            case DataType.Time:
                if (DateTime.TryParse(e.DefaultValue, out DateTime time))
                {
                    value = new LiveSheetTime(time);
                }
                break;
            case DataType.Unknown:
                break;
        }

        return value;
    }
    
}