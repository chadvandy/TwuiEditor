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
using TwUiEd.Core.Services;

namespace TwUiEd.Core.Views.Twui
{
    /// <summary>
    /// Interaction logic for ContentsFileView.xaml
    /// </summary>
    public partial class ContentsFileView : UserControl
    {
        ContentsViewModel ViewModel => (ContentsViewModel)DataContext;
        public ContentsFileView()
        {
            InitializeComponent();
            DataContext = App.Me.Services.GetService<ContentsViewModel>();
        }

        private void TabItem_Selected(object sender, RoutedEventArgs e)
        {
            if (sender is TabItem tab)
            {
                if (tab.DataContext is TwuiFileModel file)
                {
                    ViewModel.SelectTab(file);
                }
            }
        }
    }
}
