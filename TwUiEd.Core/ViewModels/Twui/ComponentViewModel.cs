using CommunityToolkit.Mvvm.ComponentModel;
using GroovyCommon.Abstractions.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TwUiEd.Core.ViewModels.Files;

namespace TwUiEd.Core.ViewModels.Twui
{
    public partial class ComponentViewModel : ViewModelBase
    {
        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Guid { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool IsSelected { get; set; } = false;

        private XElement HierarchyNode { get; set; } = null!;
        private XElement ComponentNode { get; set; } = null!;

        public ComponentViewModel? Parent { get; set; }

        //public ComponentPropertiesViewModel Properties { get; private set; } = null!;

        [ObservableProperty]
        public partial ObservableCollection<IComponentProperty> Properties { get; set; } = [];

        //  TODO vvvvv needs to be on the TwuiFileViewModel, not the Component level. Though,
        // perhaps, linking between the Component and the lines in the XML could be nice.

        // TODO Turn the contents of the XML file into a list of strings that have an
        // associated line number. Break them out into a new ComponentDocumentViewModel 
        // class that can handle editing and viewing the XML file, displaying errors, 
        // etc. etc. etc. etc. 
        //[ObservableProperty]
        //public partial 


        private TwuiFileViewModel File;

        [ObservableProperty]
        public partial ObservableCollection<ComponentViewModel> Children { get; set; } = [];

        public ComponentViewModel(TwuiFileViewModel file, XElement node, ComponentViewModel parent) : this(file, node)
        {
            Parent = parent;
        }

        public ComponentViewModel(TwuiFileViewModel file, XElement hierarchy_node)
        {
            File = file;
            HierarchyNode = hierarchy_node;

            SetupProperties();

            ParseHierarchy();

            // Find, set, and parse the "ComponentNode" that holds
            // the details of this component.
            if (HierarchyNode.Document?.Root?.Element("components")?.Element(Name) is XElement comp_node)
            {
                ComponentNode = comp_node;
                ParseComponent();
            }

            ParseChildren();
        }

        public bool HasChildren => Children.Count > 0;

        /// <summary>
        /// Setup the properties collection based on
        /// the TWUI version being used.
        /// </summary>
        private void SetupProperties()
        {
            // TODO vary based on the TWUI version.
            // TODO assign the expected XML attribute name here?

            var version = File.FileVersion;

            if (version >= 137)
            {
                Properties.Add(new ComponentBooleanPropertyViewModel("Allow Horizontal Resize", "BLOOP", "Appearance", "allowhorizontalresize", false));
                Properties.Add(new ComponentBooleanPropertyViewModel("Allow Vertical Resize", "BLOOP", "Appearance", "allowverticalresize", false));

                Properties.Add(new ComponentDockingPointPropertyViewModel("Docking Point", "", "Layout", "docking", Twui.Properties.DockingPoint.None));

                Properties.Add(new ComponentVectorPropertyViewModel("Component Offset", "", "Layout", "offset", new()));
                Properties.Add(new ComponentVectorPropertyViewModel("Dock Offset", "", "Layout", "dock_offset", new()));
            }
        }

        partial void OnIsSelectedChanged(bool value)
        {
            //throw new NotImplementedException();
            if (value) File.SelectedComponent = this;
        }

        private void ParseHierarchy()
        {
            Name = HierarchyNode.Name.LocalName;

            if (HierarchyNode.Attribute("this") is XAttribute guid)
            {
                Guid = guid.Value;
            }
        }

        private void ParseComponent()
        {
            //Properties = ComponentPropertiesViewModel.Parse(this, ComponentNode);
            ParseProperties();

            // TODO ComponentImages
            // TODO ComponentStates
            // TODO ComponentStateImageMetrics
            // TODO All other elements.
        }

        /// <summary>
        /// Parse each of the properties held in the collection,
        /// on the ComponentNode's attributes.
        /// </summary>
        private void ParseProperties()
        {
            // TODO this should probably loop through the XML Attributes first,
            // then try and find a model-driven link for a known property. That
            // way, unknown property types can be easily found.

            // Loop through every known property for this Component,
            // and try to parse it from XML.
            foreach (var prop in Properties)
            {
                // Try to find an XML Attribute with the assigned name in data.
                XAttribute? xml_attr = ComponentNode.Attribute(prop.XmlAttribute);

                if (xml_attr is null)
                {
                    // TODO errmsg of some sort.
                    continue;
                }

                if (prop.Parse(xml_attr))
                {
                    // Successful parsing of the property!
                }
            }
        }

        // Parse the children nodes of this node, if there are any, and set them
        // as children.
        private void ParseChildren()
        {
            if (HierarchyNode.HasElements)
            {
                foreach (var child in HierarchyNode.Elements())
                {
                    Children.Add(new(File, child, this));
                }
            }
        }
    }
}
