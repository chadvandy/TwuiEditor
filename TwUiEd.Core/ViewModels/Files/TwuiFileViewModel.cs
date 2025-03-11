using CommunityToolkit.Mvvm.ComponentModel;
using GroovyCommon.Abstractions.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TwUiEd.Core.ViewModels.Twui;

namespace TwUiEd.Core.ViewModels.Files
{
    public partial class TwuiFileViewModel : ViewModelBase
    {
        [ObservableProperty]
        public partial string FilePath { get; set; }

        [ObservableProperty]
        public partial string FileContents { get; set; }

        [ObservableProperty]
        public partial int FileVersion { get; private set; } = 0;

        //private ComponentViewModel Root;
        [ObservableProperty]
        public partial ComponentViewModel? SelectedComponent { get; set; }

        public ObservableCollection<ComponentViewModel> Root { get; private set; } = [];

        public async Task LoadFile(string file_path)
        {
            if (!File.Exists(file_path))
            {
                // TODO errmsg - invalid file.
                return;
            }

            // Save the name of the file.
            FilePath = file_path;

            // Asynchronously read the contents of this file.
            FileContents = await File.ReadAllTextAsync(file_path);

            // Once the contents are read, start to construct the children
            // viewmodels for the TwuiFile.

            // Create the XDocument, through which all the internal XML contents
            // will be deciphered.
            XDocument xml_doc = XDocument.Parse(FileContents);

            // The root component is the layout node (or, at least,
            // should be).
            if (xml_doc.Root is XElement layout)
            {
                if (int.TryParse(layout.Attribute("version")?.Value, out int version))
                {
                    FileVersion = version;
                }
                else
                {
                    // Errmsg; no valid version found!
                }

                // Grab the hierarchy node.
                var hierarchy = layout.Element("hierarchy");

                // TODO errmsg.
                if (hierarchy is null || !hierarchy.HasElements) return;

                // Build the Root component, which should be the one
                // and only child element of the hierarchy. Root's
                // children are created internally.
                Root.Add(new(this, hierarchy.Elements().First()));
            }
        }
    }
}
