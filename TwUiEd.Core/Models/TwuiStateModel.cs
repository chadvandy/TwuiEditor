namespace TwUiEd.Core.Models
{
    public class TwuiStateModel : TwuiBaseComponentModel
    {
        public string Name { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Interactive { get; set; }
    }
}
