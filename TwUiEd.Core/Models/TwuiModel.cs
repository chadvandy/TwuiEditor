using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace TwUiEd.Core.Models
{
    // The "internal" model for our Twui; handles a lot of the data internal to a Twui file,
    // including the version used, the hierarchy, all the individual components, etc.
    public class TwuiModel
    {
        public string Version { get => Layout.Version ?? "Version not found"; }
        public TwuiLayoutModel Layout { get; set; } = new();
        public TwuiComponentModel Root { get; set; } = new();
    }

    public class TwuiLayoutModel
    {
        public string? Version { get; set; } = string.Empty;
        public string? Comment { get; set; } = string.Empty;
        public string? PrecacheCondition { get; set; } = string.Empty;
    }

    public class TwuiBaseComponentModel
    {
        // Handles the attributes "this" and "uniqueguid"
        public string Guid { get; set; } = string.Empty;
    }

    // Model representing the entity of a single, individual Component within the hierarchy.
    public class TwuiComponentModel : TwuiBaseComponentModel
    {
        public string Name { get; set; } = string.Empty;
        public TwuiStateModel DefaultState { get; set; } = new();
        //public TwuiStateModel CurrentState { get; set; } = new();

        public TwuiComponentModel Parent { get; set; }
        public ObservableCollection<TwuiComponentModel> Children { get; set; } = [];
        public ObservableCollection<TwuiStateModel> States { get; set; } = [];
        public ObservableCollection<TwuiImageModel> Images { get; set; } = [];

        public int Depth
        {
            // Measurement function to see how far down the hierarchy tree this Component is.
            get
            {
                // Loop through all the parents until we can't find anymore!
                int d = 0;
                TwuiComponentModel parentTest = Parent;
                while (parentTest != null)
                {
                    d++;
                    parentTest = parentTest.Parent;
                }

                return d;
            }
        }

        public string HierarchyFromComponent
        {
            get {

                var tabs = new string('\t', Depth);
                var s = string.Format("{0}{1}", tabs, Name);
                

                if (Children.Count > 0)
                {
                    s += "\t>";                    
                    foreach (var item in Children)
                    {
                        s += string.Format("\n{0}", item.HierarchyFromComponent);
                    }
                }

                return s;
                //var hierarchyNames = Children
                //    .SelectMany(x => x.Name);
                //return string.Format("{0}")
                //return string.Format("{0} > \n\t {1}", Name, Children.Select(a => a.HierarchyFromComponent));
            } 
        }
    }

    public class TwuiStateModel : TwuiBaseComponentModel
    {
        public string Name { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Interactive { get; set; }
    }

    public class TwuiImageModel : TwuiBaseComponentModel
    {
        // attribute "imagepath"
        public string ImagePath { get; set; } = string.Empty;
    }

    public class TwuiStateImageMetricsModel : TwuiBaseComponentModel 
    {
        public TwuiImageModel ImageModel { get; set; } = new();
        public int Width { get; set; }
        public int Height { get; set; }

        [XmlAttribute("colour", typeof(string))]
        public Color Color { get; set; }
    }
}
