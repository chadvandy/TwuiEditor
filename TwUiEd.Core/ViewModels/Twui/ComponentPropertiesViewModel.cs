using CommunityToolkit.Mvvm.ComponentModel;
using GroovyCommon.Abstractions.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace TwUiEd.Core.ViewModels.Twui
{
    public abstract class ComponentProperty : ViewModelBase
    {
        public string Name { get; set; } = string.Empty;

        public string Tooltip { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;
    }

    public class ComponentProperty<T> : ComponentProperty
    {
        public Type ValueType => typeof(T);
        public T? Value { get; set; } = default;
        public T? DefaultValue { get; set; }

        public ComponentProperty(string name, string tooltip, string category) 
        {
            Name = name;
            Tooltip = tooltip;
            Category = category;           
        }
    }

    public class ComponentBooleanProperty : ComponentProperty<bool>
    {
        public ComponentBooleanProperty(string name, string tooltip, string category, bool default_value) : base(name, tooltip, category)
        {
            DefaultValue = default_value;
            Value = DefaultValue;
        }
    }

    /// <summary>
    /// The holder for all "properties" of a single component. This is
    /// a 1-to-1 match with ComponentViewModel, extracted out into its
    /// own class to make navigation and parsing a bit simpler.
    /// </summary>
    public partial class ComponentPropertiesViewModel : ViewModelBase
    {
        public ComponentViewModel Component { get; private set; }

        private XElement PropertiesNode = null!;

        public ComponentPropertiesViewModel(ComponentViewModel comp, XElement node)
        {
            Component = comp;
            PropertiesNode = node;

            Properties.Add(AllowHorizontalResize);
            Properties.Add(AllowVerticalResize);
        }

        [ObservableProperty]
        public partial ObservableCollection<ComponentProperty> Properties { get; set; } = [];

        // TODO Swap to a new ComponentProperty class, which determines
        // the Name & Tooltip of each ComponentProperty, and then subtype
        // that for each unique type combination.

        // Use DataTemplates in the View to programmatically display the
        // ComponentProperties in the properties view. 
        [ObservableProperty]
        public partial ComponentBooleanProperty AllowHorizontalResize { get; set; } = new("Allow Horizontal Resize", "BLOOP", "Dimensions", true);

        [ObservableProperty]
        public partial ComponentBooleanProperty AllowVerticalResize { get; set; } = new("Allow Vertical Resize", "BLOOP", "Dimensions", true);

        public static ComponentPropertiesViewModel Parse(ComponentViewModel component, XElement properties_node)
        {
            ComponentPropertiesViewModel prop = new(component, properties_node);

            // Loop through all attributes on this XElement node, and parse the
            // XML string into the expected type.
            foreach (XAttribute attr in properties_node.Attributes())
            {
                prop.ParseProperty(attr);
            }

            return prop;
        }

        private bool ParseProperty(XAttribute attr)
        {
            // TooltipsLocalised
            // CurrentState
            // DefaultState
            // Offset
            // Docking
            // DockOffset
            // ComponentAnchorPoint
            // ComponentLevelTooltip
            // TooltipLabel

            // TODO Different parsers based on the different TWUI versions?
            // TODO Different parsers based on Component vs. ComponentTemplate?
            switch (attr.Name.LocalName)
            {
                case "allowhorizontalresize":
                    AllowHorizontalResize.Value = (bool)attr;
                    break;
                case "allowverticalresize":
                    AllowVerticalResize.Value = (bool)attr;
                    break;
                default:
                    return false;
            }

            return true;
        }

        //private static bool ParseProperty(XAttribute attr)
        //{
        //    switch (attr.Name)
        //    {

        //        default:
        //            break;
        //    }


        //    return true;
        //}

        private void Parse()
        {

        }
    }
}
