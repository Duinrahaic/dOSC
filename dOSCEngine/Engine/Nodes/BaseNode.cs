using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using dOSCEngine.Utilities;
using System;

namespace dOSCEngine.Engine.Nodes
{
    public abstract class BaseNode : NodeModel
    {
        private dynamic _value = null!;
        public event Action<BaseNode>? ValueChanged;
        public delegate void PropertyChangeUpdate(string PropertyName, dynamic? Value);
        public event PropertyChangeUpdate? OnPropertyChangeUpdate;


        protected BaseNode(Point position) : base(position)
        {
            Size = new Size(1, 1);
            Initialize();
            TryInitializeProperty(PropertyType.DisplayName, string.Empty);
        }

        protected BaseNode(Guid guid, Point position) : base(position)
        {
            Guid = guid;
            Size = new Size(1, 1);
            Initialize();
            TryInitializeProperty(PropertyType.DisplayName, string.Empty);
        }

        protected BaseNode(Guid guid, Dictionary<string, dynamic> properties, Point position): base(position)
        {
            Guid = guid;
            Size = new Size(1, 1);
            Properties = properties ?? new();
            Initialize();
            TryInitializeProperty(PropertyType.DisplayName, string.Empty);

        }

        public Guid Guid { get; set; } = Guid.NewGuid();
        public virtual string NodeClass => "base";
        public virtual string Option => string.Empty;
        public string BlockHeaderClass => $"block {BlockTypeClass} {ErrorClass} {SelectedClass}";
        private string SelectedClass => Selected ? "selected" : string.Empty;
        public virtual string BlockTypeClass => string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public bool Error { get; set; } = false;
        private string ErrorClass => Error ? "error" : string.Empty;
        private DateTime _LastUpdate = DateTime.MinValue;
        public DateTime LastUpdate => _LastUpdate;
        public Dictionary<string,dynamic> Properties { get; set; } = new Dictionary<string,dynamic>();

        protected virtual void Initialize()
        {

        }


        public bool TryGetProperty<T>(string propertyName, out T value)
        {

            try
            {
                if (typeof(T).IsEnum)
                {
                    var PropertyValue = Properties[propertyName];
                    if (Enum.TryParse(PropertyValue, out T result))
                    {
                        value = result;
                        return true;
                    }
                    else
                    {
                        value = default!;
                        return false;
                    }
                }
            }
            catch
            {

            }
            

            if (Properties.TryGetValue(propertyName, out dynamic? propertyValue))
            {
                // Note: This assumes that the property with the specified name is of the correct type T
                value = (T)propertyValue;
                return true;
            }

            // Handle the case where the property doesn't exist
            value = default!;
            return false;
        }
        public T GetProperty<T>(string propertyName)
        {
            if (Properties.ContainsKey(propertyName))
            {
                // Note: This assumes that the property with the specified name is of the correct type T


                if(typeof(T).IsEnum)
                {
                    var PropertyValue = Properties[propertyName];
                    if (Enum.TryParse(PropertyValue.ToString(), out T result))
                    {
                        return result;
                    }
                }
                return (T)Properties[propertyName];
            }
            else
            {
                // Handle the case where the property doesn't exist
                throw new ArgumentException($"Property with name '{propertyName}' not found.");
            }
        }

        public void SetProperty<T>(string propertyName, T value)
        {

            if (typeof(T).IsEnum && value != null)
            {
                Properties[propertyName] = value.ToString() ;

            }
            else
            {
                // Note: This assumes that the property with the specified name is of the correct type T
                Properties[propertyName] = value!;
            }
            OnPropertyChangeUpdate?.Invoke(propertyName,value);
        }

        public bool TryInitializeProperty<T>(string propertyName, T value)
        {

            bool result = TryGetProperty<T>(propertyName, out var existingValue);
            if(result)
            {
                if (existingValue == null || value == null)
                {
                    return true; // null values are considered compatible
                }

                Type existingType = existingValue.GetType();
                Type newType = value.GetType();

                // Check for numeric compatibility
                if (Classifier.IsNumericType(existingType) && Classifier.IsNumericType(newType))
                {
                    return true;
                }

                // Check for DateTime compatibility
                if (existingType == typeof(DateTime) && newType == typeof(DateTime))
                {
                    return true;
                }

                // Check for string compatibility
                if (existingType == typeof(string) && newType == typeof(string))
                {
                    return true;
                }

                // Check for boolean compatibility
                if (existingType == typeof(bool) && newType == typeof(bool))
                {
                    return true;
                }
            }

            // Types are different or property doesn't exist, override the existing value or add a new key-value pair
            SetProperty(propertyName, value);
            return true;

        }

 

        public string GetDisplayName()
        {
            if (string.IsNullOrEmpty(GetProperty<string>(PropertyType.DisplayName)))
            {
                return GetProperty<string>(PropertyType.Name);
            }
            return GetProperty<string>(PropertyType.DisplayName);
        }
        public void ResetDisplayName()
        {
            SetProperty(PropertyType.DisplayName, string.Empty);
        }

        public void SetDisplayName(string DisplayName)
        {
            if (string.IsNullOrEmpty(DisplayName))
            {
                ResetDisplayName();
                return;
            }
            SetProperty(PropertyType.DisplayName, string.Empty);
        }


        public dynamic Value
        {
            get => _value;
            set
            {
                _value = value;
                ValueChanged?.Invoke(this);
                _LastUpdate = DateTime.Now;
            }
        }


        public void SetValue(dynamic value, bool Refresh = true)
        {
            if (Refresh)
            {
                Value = value;
            }
            else
            {
                _value = value;
            }
        }

        public virtual void CalculateValue()
        {

        }


        protected virtual dynamic GetInputValue(PortModel port, BaseLinkModel link)
        {
            var sp = (link.Source as SinglePortAnchor)!;
            var tp = (link.Target as SinglePortAnchor)!;
            var p = sp.Port == port ? tp : sp;
            try
            {
                return (p.Port.Parent as BaseNode)!.Value;
            }
            catch
            {
                return null;
            }
        }

        public virtual void ResetValue()
        {
            Value = null!;
        }

        public double InputValue(PortModel port, BaseLinkModel link)
        {
            return GetInputValue(port, link);
        }


        public BaseNodeDTO GetDTO()
        {
            return new BaseNodeDTO(this);
        }
    }
}
