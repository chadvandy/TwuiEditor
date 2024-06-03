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
    /// Interaction logic for ContentsHierarchyView.xaml
    /// </summary>
    public partial class ContentsHierarchyView : UserControl
    {
        public ContentsHierarchyView()
        {
            InitializeComponent();
            DataContext = App.Me.Services.GetService<ContentsViewModel>();
        }

        ContentsViewModel ViewModel => (ContentsViewModel)DataContext;

        private void tree_Hierarchy_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (tree_Hierarchy.SelectedItem is TwuiComponentModel component)
            {
                ViewModel.CurrentSelectedComponent = component;
            } 
        }
    }
}
