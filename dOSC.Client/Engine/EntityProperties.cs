using System.Collections.Concurrent;
using dOSC.Shared.Models.Wiresheet;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace dOSC.Client.Engine;

public class EntityProperties
{
    public delegate void PropertyChangeUpdate(EntityPropertyEnum property, dynamic? value);

    [JsonConverter(typeof(StringEnumConverter))]
    private readonly ConcurrentDictionary<EntityPropertyEnum, dynamic> _properties = new();


    public EntityProperties()
    {
        _properties = new ConcurrentDictionary<EntityPropertyEnum, dynamic>();
    }

    public EntityProperties(ConcurrentDictionary<EntityPropertyEnum, dynamic> properties)
    {
        _properties = properties ?? new ConcurrentDictionary<EntityPropertyEnum, dynamic>();
    }

    public event PropertyChangeUpdate? OnPropertyChangeUpdate;

    public ConcurrentDictionary<EntityPropertyEnum, dynamic> GetAllProperties()
    {
        return _properties;
    }

    public bool TryGetProperty<T>(EntityPropertyEnum property, out T value)
    {
        T result = default!;
        T enumResult = default!;

        if (_properties.TryGetValue(property, out var propertyValue))
        {
            if (typeof(T).IsEnum && Enum.TryParse(propertyValue.ToString(), out enumResult))
            {
                result = enumResult;
                value = result;
                return true;
            }

            if (propertyValue is T typedValue)
            {
                result = typedValue;
                value = result;
                return true;
            }
        }

        value = result;
        return false;
    }

    public T GetProperty<T>(EntityPropertyEnum property)
    {
        T result = default!;
        T enumResult = default!;
        if (_properties.TryGetValue(property, out var propertyValue))
        {
            if (typeof(T).IsEnum && Enum.TryParse(propertyValue.ToString(), out enumResult))
                result = enumResult;
            else
                result = (T)propertyValue;
        }
        else
        {
            throw new ArgumentException($"Property with name '{property.ToString()}' not found.");
        }

        return result;
    }

    public void SetProperty<T>(EntityPropertyEnum property, T value)
    {
        if (typeof(T).IsEnum && value != null)
            _properties[property] = value.ToString();
        else
            _properties[property] = value!;

        OnPropertyChangeUpdate?.Invoke(property, value);
    }


    public void TryInitializeProperty<T>(EntityPropertyEnum property, T value)
    {
        var result = TryGetProperty(property, out T existingValue);

        if (value == null)
            return;

        // If the property doesn't exist
        if (!result || existingValue == null)
        {
            // Handle the case where existingValue is null
            SetProperty(property, value);
            return;
        }

        // If types are different, return false
        if (existingValue.GetType() != value.GetType())
        {
            SetProperty(property, value);
        }
    }


    public string GetDTO()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}