using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwUiEd.Core.Attributes;
using TwUiEd.Core.Models;

namespace TwUiEd.Core.Views.Twui
{
    /// <summary>
    /// Interaction logic for ContentsPropertiesView.xaml
    /// </summary>
    public partial class ContentsPropertiesView : UserControl
    {
        ContentsViewModel ViewModel { get; }
        public ContentsPropertiesView()
        {
            InitializeComponent();

            ViewModel = App.Me.Services.GetService<ContentsViewModel>();
            DataContext = ViewModel;

            //ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += (o, e) =>
            {
                //Debug.WriteLine("Main VM Property Changed");
                switch (e.PropertyName)
                {
                    case "CurrentSelectedComponent":
                        // Refresh and setup our listbox whenever we grab a new selected component in the left-side
                        // of our User Interface.
                        SetupListBox();
                        break;
                }
            };
        }

        private void SetupListBox()
        {
            var lb = PropertiesListBox;
            var items = lb.Items;
            items.Clear();

            // Run through our currently selected component from our ContentsViewModel and
            // populate our list box using every non-null TwuiProperty within the currently selected component.
            TwuiComponentModel? SelectedModel = ViewModel.CurrentSelectedComponent;

            if (SelectedModel != null)
            {
                // Grab every TwuiPropertyModel in our UndecodedProperties holder.
                foreach (var undecoded_prop in SelectedModel.UndecodedProperties)
                {
                    items.Add(undecoded_prop);
                }

                // Loop through all of our TwuiPropertyAttribute'd fields within this component.
                var props = SelectedModel.GetType().GetProperties();
                foreach (var prop_info  in props)
                {
                    if (prop_info.IsDefined(typeof(TwuiPropertyAttribute), false))
                    {
                        var prop_getter = prop_info.GetGetMethod();
                        var prop_def = prop_getter?.Invoke(SelectedModel, []);
                        if (prop_def is ITwuiPropertyModel twuiProperty && !twuiProperty.IsNull())
                        { 
                            items.Add(twuiProperty);
                        }
                    } 
                }
            }
        }
    }

    // Selector to determine which input resource and UI layout we'll pick, based on each individual
    // TwuiProperty.
    public class TwuiPropertyDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            // Grab the root of our XAML element, which holds the resources we'll pick from
            // to choose the UI layout and resources.
            FrameworkElement element = (FrameworkElement)container;

            if (element != null && item != null)
            {
                //if (item is ITwuiPropertyModel<dynamic> prop) {
                if (item is ITwuiPropertyModel prop) {
                    Type type = prop.DataType;
                    string type_name = type.Name;

                    DataTemplate? dataTemplate;

                    if (!prop.IsDecoded)
                    {
                        // Get a special resource for any Undecoded properties.
                        dataTemplate = element.TryFindResource("Undecoded") as DataTemplate;
                    }
                    else
                    {
                        // Try and grab a resource with the name of the type we've provided.
                        dataTemplate = element.TryFindResource(type_name) as DataTemplate;
                    }

                    if (dataTemplate != null)
                    {
                        return dataTemplate;
                    }
                }
            }

            // Default to Undecoded.
            return element.FindResource("Undecoded") as DataTemplate;
            //return base.SelectTemplate(item, container);
        }
    }
}
