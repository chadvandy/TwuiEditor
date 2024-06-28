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
using TwUiEd.Core.Models;

namespace TwUiEd.Core.Views.Twui
{
    /// <summary>
    /// Interaction logic for ContentsPropertiesView.xaml
    /// </summary>
    public partial class ContentsPropertiesView : UserControl
    {
        public ContentsPropertiesView()
        {
            InitializeComponent();
            DataContext = App.Me.Services.GetService<ContentsViewModel>();
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
                if (item is TwuiPropertyModel prop) {
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
