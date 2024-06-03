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
}
