using CommunityToolkit.Mvvm.ComponentModel;
using GroovyCommon.Abstractions.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwUiEd.Core.ViewModels.Files
{
    public partial class TwuiFileViewModel : ViewModelBase
    {
        [ObservableProperty]
        public partial string FilePath { get; set; }

        public string FileContents;

        // TODO 

        //public TwuiFileViewModel() : this("") { }

        public TwuiFileViewModel(string file_path)
        {
            if (!File.Exists(file_path))
            {
                // errmsg - invalid file!
                //return;
            }
            // Save the name of the file in question.
            FilePath = file_path;

            // TODO Stream the contents of the file and hold it internally.

            // TODO Read the text "asynchronously" and assign it to a public property, so the contents can be read in 
            // the background without blocking the UI thread.
            FileContents = File.ReadAllText(file_path);
        }
    }
}
