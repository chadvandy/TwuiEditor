using CommunityToolkit.Mvvm.ComponentModel;
using GroovyCommon.Abstractions.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using TwUiEd.Core.ViewModels.Twui.Properties;

namespace TwUiEd.Core.ViewModels.Twui
{
    public partial class VectorViewModel : ViewModelBase
    {
        private Vector2 Vector { get; set; }

        public VectorViewModel(Vector2 vector)
        {
            Vector = vector;

            X = Vector.X;
            Y = Vector.Y;
        }

        [ObservableProperty]
        public partial float X { get; set; }

        [ObservableProperty]
        public partial float Y { get; set; }

        partial void OnXChanged(float value)
        {
            Vector = Vector2.Add(Vector, new Vector2(value, 0));
        }

        partial void OnYChanged(float value)
        {
            Vector = Vector2.Add(Vector, new Vector2(0, value));
        }
    }

    public interface IComponentProperty
    {
        public string Name { get; set; }
        public string Tooltip { get; set; }
        public string Category { get; set; }
        public string XmlAttribute { get; set; }

        public bool Parse(XAttribute attr);
    }

    public abstract partial class ComponentPropertyViewModel<T> : ViewModelBase, IComponentProperty
    {
        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Tooltip { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Category { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string XmlAttribute { get; set; } = string.Empty;

        [ObservableProperty]
        public partial T Value { get; set; } = default;

        [ObservableProperty]
        public partial T DefaultValue { get; private set; } = default;

        [ObservableProperty]
        public partial Type ValueType { get; private set; } = typeof(T);

        public abstract bool Parse(XAttribute attr);

        public ComponentPropertyViewModel(string name, string tt, string cat, string attr, Type type, T default_value)
        {
            Name = name;
            Tooltip = tt;
            Category = cat;
            XmlAttribute = attr;

            ValueType = type;
            DefaultValue = default_value;
            Value = default_value;
        }
    }

    public class ComponentBooleanPropertyViewModel : ComponentPropertyViewModel<bool>
    {
        public ComponentBooleanPropertyViewModel(string name, string tooltip, string category, string attr, bool default_value)
            : base(name, tooltip, category, attr, typeof(bool), default_value) { }

        public override bool Parse(XAttribute attr)
        {
            Value = (bool)attr;

            return true;
        }
    }

    public class ComponentStringPropertyViewModel : ComponentPropertyViewModel<string>
    {
        public ComponentStringPropertyViewModel(string name, string tt, string cat, string attr, string default_value) 
            : base(name, tt, cat, attr, typeof(string), default_value)
        {
        }

        public override bool Parse(XAttribute attr)
        {
            Value = (string)attr;
            
            return true;
        }
    }

    public partial class ComponentVectorPropertyViewModel : ComponentPropertyViewModel<Vector2>
    {
        public ComponentVectorPropertyViewModel(string name, string tt, string cat, string attr, Vector2 default_value) : base(name, tt, cat, attr, typeof(Vector2), default_value)
        {
        }

        [ObservableProperty]
        public partial float X { get; set; }

        [ObservableProperty]
        public partial float Y { get; set; }

        public override bool Parse(XAttribute attr)
        {
            // TODO The attribute *should* be a string with two numbers
            // split by a comma.
            //throw new NotImplementedException();

            //Vector2 val = new();

            string val = attr.Value;

            var numbers = val.Split(",", 2);

            if (numbers.Length != 2)
            {
                // TODO errmsg, can't parse.
                return false;
            }


            Vector2 ret = new();

            // Convert each split string into
            // a number to put into Vec2.
            if (!float.TryParse(numbers[0], out float num_1))
            {
                // Failed to parse the X value.
                // TODO errmsg.
                return false;
            }

            if (!float.TryParse(numbers[1], out float num_2))
            {
                // Failed to parse the Y value.
                // TODO errmsg.
                return false;
            }

            ret.X = num_1;
            ret.Y = num_2;

            Value = ret;

            X = Value.X;
            Y = Value.Y;

            return true;
        }
    }

    public class ComponentDockingPointPropertyViewModel : ComponentPropertyViewModel<DockingPoint>
    {
        public ComponentDockingPointPropertyViewModel(string name, string tt, string cat, string attr, DockingPoint default_value) : base(name, tt, cat, attr, typeof(DockingPoint), default_value)
        {
        }

        public override bool Parse(XAttribute attr)
        {
            // TODO Find the enum that matches the text of the XML node.
            //throw new NotImplementedException();

            Value = DockingPoint.None;

            return true;
        }
    }
}
