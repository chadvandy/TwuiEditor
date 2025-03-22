using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GroovyCommon.Abstractions.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TwUiEd.Core.Models;
using TwUiEd.Core.Services;
using TwUiEd.Core.ViewModels.Files;
using TwUiEd.Core.Views.Twui;

namespace TwUiEd.Core.ViewModels.Windows
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        public TwuiFileModel? openedFile;

        [ObservableProperty]
        public partial TwuiFileViewModel? CurrentlyDisplayedFile { get; set; }

        [ObservableProperty]
        public partial ObservableCollection<TwuiFileViewModel> OpenedFiles { get; set; } = [];

        //[ObservableProperty]

        [ObservableProperty]
        public partial string StatusText { get; set; } = string.Empty;

        //[ObservableProperty]
        //public partial bool Maximized { get; set; } = true;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MaximizeWindowCommand))]
        [NotifyCanExecuteChangedFor(nameof(RestoreWindowCommand))]
        public partial WindowState WindowState { get; set; } = System.Windows.WindowState.Maximized;

        //public IList<CommandViewModel> MenuBarCommands { get; set; }

        public MainWindowViewModel()
        {
            DisplayName = "TWUI Editor";
        }

        private async Task SetOpenedFile(string file_path)
        {
            // Construct the new TwuiFileViewModel and then run
            // its own async command to process the file contents and
            // build its innards.
            TwuiFileViewModel file_vm = new();

            OpenedFiles.Add(file_vm);
            CurrentlyDisplayedFile = file_vm;

            await file_vm.LoadFile(file_path);

            //var vm = await TwuiFileViewModel.LoadTwuiFile(file_path);

            //await TwuiFileViewModel.LoadTwuiFile(file_path);
        }

        //public static bool IsMaximized()
        //{
        //    return App.Current.MainWindow.WindowState == System.Windows.WindowState.Maximized;
        //}

        //public static bool IsNotMaximized()
        //{
        //    return !IsMaximized();
        //}
        
        public bool IsMaximized()
        {
            return WindowState == WindowState.Maximized;
        }

        public bool IsNotMaximized()
        {
            return WindowState != WindowState.Maximized;
        }

        [RelayCommand]
        private void CloseWindow()
        {
            SystemCommands.CloseWindow(App.Current.MainWindow);
        }

        [RelayCommand(CanExecute = nameof(IsNotMaximized))]
        private void MaximizeWindow()
        {
            SystemCommands.MaximizeWindow(App.Current.MainWindow);

            WindowState = WindowState.Maximized;
        }

        [RelayCommand]
        private void MinimizeWindow()
        {
            SystemCommands.MinimizeWindow(App.Current.MainWindow);

            WindowState = WindowState.Minimized;
        }


        [RelayCommand(CanExecute = nameof(IsMaximized))]
        private void RestoreWindow()
        {
            SystemCommands.RestoreWindow(App.Current.MainWindow);

            WindowState = WindowState.Normal;
        }


        [RelayCommand]
        private async Task OpenFile()
        {
            // Setup a file dialog to open a file on disk.

            // TODO On file selected, add view model to the list of current files,
            // and set the new file as the displayed view model.

            string file_path = string.Empty;

            OpenFileDialog fileDialog = new()
            {
                Filter = "TWUI.XML files (*.twui.xml)|*.twui.xml",
                FilterIndex = 0,
                RestoreDirectory = true
            };

            if (fileDialog.ShowDialog() == true)
            {
                // If the user pressed "Ok" and had a file selected that
                // isn't empty, go forward with creating a new ViewModel
                // with this file path, and set that as the currently
                // displayed file.
                file_path = fileDialog.FileName;

                if (!string.IsNullOrEmpty(fileDialog.FileName))
                {
                    await SetOpenedFile(fileDialog.FileName);
                    //TwuiFileViewModel opened_file = new(fileDialog.FileName);

                    //OpenedFiles.Add(opened_file);
                    //CurrentlyDisplayedFile = opened_file;
                }
                ////Read the contents of the file into a stream
                //var fileStream = fileDialog.OpenFile();

                //using StreamReader reader = new(fileStream);
                //fileContent = reader.ReadToEnd();
            }

            //// TODO create a class in the background to handle the TwuiFile
            //// TODO hold the class in memory, in a way that can scale to having more than one open at a time
            //IFileService fileService = App.Current.Services.GetService<IFileService>();
            //var twui = fileService?.OpenFile(filePath, fileContent);

            ////var main_vm = App.Me.Services.GetService<MainWindowViewModel>();
            ////main_vm.OpenedFile = twui;

            //if (twui != null)
            //{
            //    var cvm = App.Current.Services.GetService<ContentsViewModel>();
            //    cvm?.OpenedFiles.Add(twui);
            //    cvm?.SelectTab(twui);
            //}


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

        //public void OnOpenedFileChanged(TwuiFileModel? value) {
        //}
    }
}
