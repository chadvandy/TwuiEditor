﻿using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwUiEd.Core.Models;
using TwUiEd.Core.Services;

namespace TwUiEd.Core.Views.Twui
{
    public partial class ContentsViewModel : ObservableRecipient
    {
        public ObservableCollection<TwuiFileModel> OpenedFiles { get; set; } = [];

        [ObservableProperty]
        public TwuiFileModel? currentSelectedFile;

        [ObservableProperty]
        public TwuiComponentModel? currentSelectedComponent;

        public string Hierarchy { 
            get {
                return CurrentSelectedFile?.Model.Root.HierarchyFromComponent ?? "No file selected";
            }
        }

        public string Contents { get; set; } = string.Empty;

        private IFileService _service;

        public void SelectTab(TwuiFileModel file)
        {
            CurrentSelectedFile = file;
        }

        public ContentsViewModel(IFileService fileService)
        {
            _service = fileService;

            //main_vm.PropertyChanging += (o, e) =>
            //{
            //    Debug.WriteLine("Main VM Property Changing");
            //};

            //main_vm.OnOpenedFileChanged += _service_PropertyChanged;
            //main_vm.PropertyChanged += (o, e) =>
            //{
            //    Debug.WriteLine("Main VM Property Changed");
            //    switch (e.PropertyName)
            //    {
            //        case "OpenedFile": 
            //            Model = main_vm.OpenedFile;
            //            Contents = Model?.FileContents ?? string.Empty;

            //            break;
            //    }
            //};
            //main_vm.PropertyChanged += _service_PropertyChanged;

            //_service.PropertyChanged += _service_PropertyChanged;
            //_service.PropertyChanged += _service_PropertyChanged;
        }

        private void _service_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            //Model = _service.OpenedFile;
        }
    }
}
