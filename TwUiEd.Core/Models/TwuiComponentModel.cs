using System.Collections.ObjectModel;
using TwUiEd.Core.Attributes;

namespace TwUiEd.Core.Models
{
    // Model representing the entity of a single, individual Component within the hierarchy.
    public class TwuiComponentModel : TwuiBaseComponentModel
    {
        public string Name { get; set; } = string.Empty;

        [TwuiProperty("id", Description = "The in-game hierarchical key for this Component. Ids must be unique between sibling components.", Name = "Component Id", Required = true)]
        public TwuiPropertyModel<string> Id { get; set; } = new(string.Empty);

        [TwuiProperty("tooltipslocalised")]
        public TwuiPropertyModel<bool> TooltipsLocalised { get; set; } = new(false);

        [TwuiProperty("clipchildren", Required = false,
            Description = "Prevent child components from being drawn if they're outside of this component's bounds.")]
        public TwuiPropertyModel<bool> ClipChildren { get; set; } = new(false);

        //public TwuiPropertyModel<ValueTuple<float, float>> Offset { get; set; } = new((0.0f,0.0f));
        [TwuiProperty("offset", Required =false)]
        public TwuiPropertyModel<ValueTuple<float, float>> Offset { get; set; } = new((0.0f,0.0f));

        public TwuiStateModel DefaultState { get; set; } = new();
        //public TwuiStateModel CurrentState { get; set; } = new();

        public TwuiComponentModel? Parent { get; set; }

        public ObservableCollection<TwuiComponentModel> Children { get; set; } = [];

        [TwuiElement("states", typeof(TwuiStateModel), Name="States", IsList=true, Required=true)]
        public ObservableCollection<TwuiStateModel> States { get; set; } = [];

        [TwuiElement("componentimages", typeof(TwuiImageModel), Name="Images", IsList=true, Required=true)]
        public ObservableCollection<TwuiImageModel> Images { get; set; } = [];

        //public ObservableCollection<Tuple<string, string>> Properties { get; set; } = [];

        // Undecoded properties.
        public ObservableCollection<TwuiPropertyModel<string>> UndecodedProperties { get; set; } = [];

        //public ObservableCollection<TwuiPropertyModel> AllProperties { 
        //    get 
        //    {
        //        var allProps = new ObservableCollection<TwuiPropertyModel>();
        //        var props = GetType().GetProperties();

        //        foreach (var prop_info in props)
        //        {
        //            if (prop_info.IsDefined(typeof(TwuiPropertyAttribute), false))
        //            {
        //                var prop_getter = prop_info.GetGetMethod();
        //                var prop_default = prop_getter?.Invoke(this, []);
        //                //TwuiPropertyModel<dynamic>? prop = prop_getter?.Invoke(this, []) as TwuiPropertyModel<dynamic>;
        //                if (prop_default != null)
        //                {
        //                    TwuiPropertyModel prop = (TwuiPropertyModel)prop_default;

        //                    if (prop.Value != null)
        //                    {
        //                        prop.PropertyName = prop_info.Name;
        //                        allProps.Add(prop);
        //                    }
        //                }
        //            }
        //        }

        //        foreach (TwuiPropertyModel undecoded_prop in Properties)
        //        {
        //            if (undecoded_prop.Value != null) 
        //            {
        //                allProps.Add(undecoded_prop);
        //            }
        //        }

        //        return allProps;
        //    }
        //    set 
        //    {
        //        // Loop through all of our TwuiPropertyModels passed back to AllProperties,
        //        // and run through and apply any changes to the Value field to the TwuiPropertyModel internally.
        //        foreach (var item in value)
        //        {
        //            if (item.IsDecoded)
        //            {
        //                var prop_name = item.PropertyName;
        //                var prop_info = GetType().GetProperty(prop_name);

        //                var prop_setter = prop_info?.GetSetMethod();
        //                prop_setter?.Invoke(this, [item]);
        //            }
        //            else
        //            {
        //                var undecoded_prop = Properties.FirstOrDefault(x => x.Name == item.Name);
        //                if (undecoded_prop is TwuiPropertyModel twui_prop)
        //                {
        //                    twui_prop.Value = item.Value;
        //                }
        //            }
        //        }
        //    }
        //}

        public int Depth
        {
            // Measurement function to see how far down the hierarchy tree this Component is.
            get
            {
                // Loop through all the parents until we can't find anymore!
                int d = 0;
                TwuiComponentModel? parentTest = Parent;
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
}
