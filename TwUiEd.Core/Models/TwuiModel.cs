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
        public string Version { get => Layout.Version; }
        public TwuiLayoutModel Layout { get; set; } = new();
        public TwuiComponentModel Root { get; set; } = new();
    }

    public class TwuiLayoutModel
    {
        public string Version { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public string PrecacheCondition { get; set; } = string.Empty;
    }

    public class TwuiBaseComponentModel
    {
        // Handles the attributes "this" and "uniqueguid"
        public Guid Id { get; set; }
    }

    // Model representing the entity of a single, individual Component within the hierarchy.
    public class TwuiComponentModel : TwuiBaseComponentModel
    {
        public string Name { get; set; } = string.Empty;
        public TwuiStateModel DefaultState { get; set; } = new();
        //public TwuiStateModel CurrentState { get; set; } = new();

        public ObservableCollection<TwuiComponentModel> Children { get; set; } = [];
        public ObservableCollection<TwuiStateModel> States { get; set; } = [];
        public ObservableCollection<TwuiImageModel> Images { get; set; } = [];
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
