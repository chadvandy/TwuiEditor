//using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using TwUiEd.Core.Views.Twui;

namespace TwUiEd.Core.Views.Global
{
    /// <summary>
    /// Interaction logic for MenuBar.xaml
    /// </summary>
    public partial class MenuBar : UserControl
    {
        public MenuBar()
        {
            InitializeComponent();
        }

        private void btn_Files_Open_Click(object sender, RoutedEventArgs e)
        {
            // TODO Setup a file dialog to open a file on disk.
            // TODO filter to only accept .twui.xml files
            var fileContent = string.Empty;
            var filePath = string.Empty;

            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "TWUI.XML files (*.twui.xml)|*.twui.xml";
            fileDialog.FilterIndex = 0;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == true)
            {
                filePath = fileDialog.FileName;

                //Read the contents of the file into a stream
                var fileStream = fileDialog.OpenFile();

                using StreamReader reader = new(fileStream);
                fileContent = reader.ReadToEnd();
            }

            // TODO create a class in the background to handle the TwuiFile
            // TODO hold the class in memory, in a way that can scale to having more than one open at a time
            IFileService fileService = App.Me.Services.GetService<IFileService>();
            var twui = fileService?.OpenFile(filePath, fileContent);

            //var main_vm = App.Me.Services.GetService<MainWindowViewModel>();
            //main_vm.OpenedFile = twui;

            if (twui  != null)
            {
                var cvm = App.Me.Services.GetService<ContentsViewModel>();
                cvm?.OpenedFiles.Add(twui);
            }


            // TODO create the ContentsView and ship the info for its ViewModel.
            //var contentsView = MainWindow:

            // TODO determine the version of the TwuiFile
            // TODO decipher the hierarchy of the componets within the file
            // TODO show properties in side-bar when selecting a component within
            // TODO ability to edit properties in side-bar when selecting a component
            // TODO stash those edits in the held class, show if a file has been edited at all while open, and either have the ability
            // to "revert changes" (ie., reread the twui file), or the ability to save those changes to the file itself
            // TODO view what the component would actually look like in another control, with some simple zoom settings
            // TODO setting window to determine your "game data location", which we can use to grab the UI assets from within
            // the game packs, and we can use that to more cleanly show the component view
            // TODO we can also either use the SteamAPI to get install location, or we can deduce it using some simple
            // heuristics.
            // TODO view TwuiFiles from within a PackFile, but saving those requires a new output to start.
        }

        private void CommandOpen_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }
    }
}
