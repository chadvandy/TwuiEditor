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

        public ComponentPropertiesViewModel Properties { get; private set; } = null!;

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

        public bool HasChildren => Children.Count > 0;

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
            Properties = ComponentPropertiesViewModel.Parse(this, ComponentNode);

            // TODO ComponentImages
            // TODO ComponentStates
            // TODO ComponentStateImageMetrics
            // TODO All other elements.
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

        public ComponentViewModel(TwuiFileViewModel file, XElement hierarchy_node)
        {
            File = file;
            HierarchyNode = hierarchy_node;

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
    }
}
