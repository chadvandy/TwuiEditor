using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TwUiEd.Core.Models
{
    //public interface ITwuiPropertyModel
    //{
    //    Type DataType { get; }
    //    string Description { get; set; }
    //    bool IsDecoded { get; set; }
    //    bool IsRequired { get; set; }
    //    string Name { get; set; }

    //    string PropertyName { get; set; }
    //}

    //public interface ITwuiPropertyModel<T> : ITwuiPropertyModel
    //{
    //    T DefaultValue { get; }
    //    T? Value { get; set; }
    //}


    //public interface ITwuiPropertyModel<out T> : ITwuiPropertyModel
    //{
    //    public T DefaultValue { get; }
    //    public T Value { get; }
    //}

    //public interface ITwuiPropertyModel
    //{
    //    public Type DataType { get; }
    //    public string Description { get; set; }
    //    public bool IsDecoded { get; set; }
    //    public bool IsRequired { get; set; }
    //    public string Name { get; set; }
    //    public string PropertyName { get; set; }
    //}

    //public interface ITwuiPropertyModel<T> : ITwuiPropertyModel
    //{
    //    public T DefaultValue { get; }
    //    public T Value { get; }
    //}

    //public struct TwuiPropertyString(string val)
    //{
    //    public string Value { get; set; } = val;

    //    public override readonly string ToString() => Value;

    //    public static implicit operator string(TwuiPropertyString tps) => tps.Value;
    //    public static implicit operator TwuiPropertyString(string s) => new(s);
    //}

    //public abstract class TwuiPropertyModel
    //{
    //    public abstract Type DataType { get; }

    //    public dynamic DefaultValue { get; }
    //    public dynamic? Value { get; set;  }

    //    public string Description { get; set; } = string.Empty;
    //    public bool IsDecoded { get; set; } = true;
    //    public bool IsRequired { get; set; } = false;
    //    public string Name { get; set; } = string.Empty;
    //    public string PropertyName { get; set; } = string.Empty;
    //}

    public class TwuiPropertyModel(Type type, dynamic default_value)
    {
        public Type DataType { get; } = type;

        public dynamic DefaultValue { get; set; } = default_value;
        public dynamic? Value { get; set; }

        public string Description { get; set; } = string.Empty;
        public bool IsDecoded { get; set; } = true;
        public bool IsRequired { get; set; } = false;
        public string Name { get; set; } = string.Empty;
        public string PropertyName { get; set; } = string.Empty;
    }

    //public class OldTwuiPropertyModel<T>(T default_value) : ITwuiPropertyModel<T>
    //{
    //    public Type DataType { get => typeof(T); }
        
    //    public T DefaultValue { get; set; } = default_value;
    //    public T Value { get; set; } = default_value;

    //    public string Description { get; set; } = string.Empty;
    //    public bool IsDecoded { get; set; } = true;
    //    public bool IsRequired { get; set; } = false;
    //    public string Name { get; set; } = string.Empty;
    //    public string PropertyName { get; set; } = string.Empty;
    //}

    //public class _OLDTwuiPropertyModel<T>(T default_value) : ITwuiPropertyModel<T>
    //{
    //    // A property on any Twui Element (whether it's Component, State, etc.)
    //    // We need to be mindful of the desired type of the value, and the name of the property,
    //    // as well as, of course, the current value.
    //    public string Name { get; set; } = string.Empty;

    //    //public Type PropertyType { get; set; } = type;

    //    // The name of the getter/setter property for this field within an element.
    //    public string PropertyName { get; set; } = string.Empty;

    //    public Type DataType
    //    {
    //        get { return typeof(T); }
    //    }

    //    public T DefaultValue { get; private set; } = default_value;
    //    public T? Value { get; set; }

    //    public bool IsRequired { get; set; } = false;
    //    public bool IsDecoded { get; set; } = true;
    //    public string Description { get; set; } = string.Empty;
    //}
}
