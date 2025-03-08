using System.CodeDom;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Numerics;

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

    public interface ITwuiPropertyModel
    {
        public Type DataType { get; }
        public string Description { get; set; }
        public bool IsDecoded { get; set; }
        public bool IsRequired { get; set; }
        public string Name { get; set; }
        public string PropertyName { get; set; }

        public bool IsNull();
        public bool TryParse(string value);
        //public bool TryParse(string value, out Type type);
    }

    public interface ITwuiPropertyModel<T> : ITwuiPropertyModel
    {
        public T DefaultValue { get; }
        public T? Value { get; }
    }

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

    //public class OldTwuiPropertyModel(Type type, dynamic default_value)
    //{
    //    public Type DataType { get; } = type;

    //    public dynamic DefaultValue { get; set; } = default_value;
    //    public dynamic? Value { get; set; }

    //    public string Description { get; set; } = string.Empty;
    //    public bool IsDecoded { get; set; } = true;
    //    public bool IsRequired { get; set; } = false;
    //    public string Name { get; set; } = string.Empty;
    //    public string PropertyName { get; set; } = string.Empty;
    //}

    public interface ITwuiDataConverter
    {
        public bool CanParse(string value);
        //public void DecodeData(string value);
        public string EncodeData();
        //public dynamic Value { get; }
    }

    public interface ITwuiDataConverter<T> : ITwuiDataConverter
    {
        public T Value { get; set; }
    }

    //public class TwuiDataConverter<T> : ITwuiDataConverter
    //{
    //    public T ITwuiDataConverter.Value { get {  return (T)this.Value; } }

    //    public T Value { get; }

    //    public bool CanParse(string value)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void DecodeData(string value)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public string EncodeData()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    // TODO create a nice decoder for tuple strings to tuples.
    public abstract class TupleDataConverter : ITwuiDataConverter
    {
        // A data converter for XML strings to an arbitrary Tuple.

        public abstract bool CanParse(string value);
        public abstract string EncodeData();
        public abstract ValueTuple DecodeData(string value);
    }

    public abstract class TupleDataConverter<T1, T2> : TupleDataConverter
    {
        // Two value tuple.
        public Tuple<T1, T2> Value { get; set; }
        //public abstract override Tuple<T1, T2> DecodeData(string value);
    }

    //public class FloatTupleDataConverter : TupleDataConverter<float, float>
    //{
    //    // Converter between a string with two floats and a tuple w/ two floats.
    //    // ie., "50.00,10.00" <--> (50.0f, 10.0f)
    //    public override bool CanParse(string value)
    //    {
    //        if (!value.Contains(','))
    //        {
    //            return false;
    //        } 
    //        if (value.Length < 3)
    //        {
    //            return false;
    //        }

    //        return true;
    //    }

    //    //public override ValueTuple<float,float> DecodeData(string value)
    //    //{
    //    //    // Split up the tuple into discrete chunks and try to convert each discrete chunk in our tuple.
    //    //    string[] tuple_split = value.Split(',');
    //    //    if (tuple_split.Length != 2)
    //    //    {
    //    //        return (0,0);
    //    //    }

    //    //    string val1 = tuple_split[0];
    //    //    string val2 = tuple_split[1];

    //    //    float f1 = float.Parse(val1);
    //    //    float f2 = float.Parse(val2);

    //    //    return new (f1, f2);
    //    //}

    //    public override string EncodeData()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class TwuiPropertyModel<T>(T default_value) : ITwuiPropertyModel<T>
    {
        public Type DataType { get => typeof(T); }

        public T DefaultValue { get; } = default_value;
        public T? Value { get; set; }

        //public ITwuiDataConverter Converter { get; set; }

        public bool IsNull()
        {
            return (Name == string.Empty || Value == null);
        }
        public bool TryParse(string value)
        {
            try
            {
                //    switch (Value)
                //    {
                //        case Tuple<float, float> ftup:
                //            var Converter = new FloatTupleDataConverter();
                //            //Converter.DecodeData(value);
                //            Value = Converter.DecodeData(ftup);
                //            //Value = ftup;
                //            //Value = new FloatTupleDataConverter().DecodeData(value);
                //            break;
                //        default:
                //            Value = (T)Convert.ChangeType(value, typeof(T));
                //            break;
                //    }
            } 
            catch (Exception e)
            {
                Debug.Print($"Failed at parsing! Error: {e.Message}");
            }

            return !IsNull();
        }

        public string Description { get; set; } = string.Empty;
        public bool IsDecoded { get; set; } = true;
        public bool IsRequired { get; set; } = false;
        public string Name { get; set; } = string.Empty;
        public string PropertyName { get; set; } = string.Empty;
    }

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
