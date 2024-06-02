using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwUiEd.Core.Models;

namespace TwUiEd.Core.Views.Global
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        public TwuiFileModel? openedFile;



        //public void OnOpenedFileChanged(TwuiFileModel? value) {
        //}
    }
}
