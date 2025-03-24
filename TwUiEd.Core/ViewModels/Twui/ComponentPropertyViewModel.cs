using CommunityToolkit.Mvvm.ComponentModel;
using GroovyCommon.Abstractions.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TwUiEd.Core.ViewModels.Twui.Properties;

namespace TwUiEd.Core.ViewModels.Twui
{
    public abstract partial class ComponentPropertyViewModel : ViewModelBase
    {
        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Tooltip { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Category { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string XmlAttribute { get; set; } = string.Empty;

        public abstract bool Parse(XAttribute attr);
    }

    public abstract partial class ComponentPropertyViewModel<T> : ComponentPropertyViewModel
    {
        public Type ValueType => typeof(T);

        [ObservableProperty]
        public partial T Value { get; set; }

        [ObservableProperty]
        public partial T DefaultValue { get; set; }

        public ComponentPropertyViewModel(string name, string tt, string cat, string attr, T default_value)
        {
            Name = name;
            Tooltip = tt;
            Category = cat;
            XmlAttribute = attr;

            DefaultValue = default_value;
            Value = default_value;
        }

        public override abstract bool Parse(XAttribute attr);
    }

    public class ComponentBooleanPropertyViewModel : ComponentPropertyViewModel<bool>
    {
        public ComponentBooleanPropertyViewModel(string name, string tooltip, string category, string attr, bool default_value)
            : base(name, tooltip, category, attr, default_value) { }

        public override bool Parse(XAttribute attr)
        {
            Value = (bool)attr;

            return true;
        }
    }

    public class ComponentStringPropertyViewModel : ComponentPropertyViewModel<string>
    {
        public ComponentStringPropertyViewModel(string name, string tt, string cat, string attr, string default_value) 
            : base(name, tt, cat, attr, default_value)
        {
        }

        public override bool Parse(XAttribute attr)
        {
            Value = (string)attr;
            
            return true;
        }
    }

    public class ComponentVectorPropertyViewModel : ComponentPropertyViewModel<Vector2>
    {
        public ComponentVectorPropertyViewModel(string name, string tt, string cat, string attr, Vector2 default_value) : base(name, tt, cat, attr, default_value)
        {
        }

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

            return true;
        }
    }

    public class ComponentDockingPointPropertyViewModel : ComponentPropertyViewModel<DockingPoint>
    {
        public ComponentDockingPointPropertyViewModel(string name, string tt, string cat, string attr, DockingPoint default_value) : base(name, tt, cat, attr, default_value)
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
