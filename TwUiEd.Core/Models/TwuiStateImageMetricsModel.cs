using System.Windows.Media;
using System.Xml.Serialization;

namespace TwUiEd.Core.Models
{
    public class TwuiStateImageMetricsModel : TwuiBaseComponentModel 
    {
        public TwuiImageModel ImageModel { get; set; } = new();
        public int Width { get; set; }
        public int Height { get; set; }

        [XmlAttribute("colour", typeof(string))]
        public Color Color { get; set; }
    }
}
