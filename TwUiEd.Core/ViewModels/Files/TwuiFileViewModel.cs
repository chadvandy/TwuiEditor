using CommunityToolkit.Mvvm.ComponentModel;
using GroovyCommon.Abstractions.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml;
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

        [ObservableProperty]
        public partial ComponentViewModel? SelectedComponent { get; set; }

        // TODO A list of all the lines in the XML document.
        public ObservableCollection<string> DocumentLines { get; set; } = [];

        [ObservableProperty]
        public partial FlowDocument XmlDocument { get; set; } = new();

        // Using an ObservableCollection to bind the root to a TreeView;
        // using a single data context on a treeview is unsupported.
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

            DocumentLines = [.. FileContents.Split(Environment.NewLine, StringSplitOptions.None)];

            // Once the contents are read, start to construct the children
            // viewmodels for the TwuiFile.

            // TODO Clean up the FileContents, removing any XML comments which can ruin interpretation.

            // Create the XDocument, through which all the internal XML contents
            // will be deciphered.
            XDocument xml_doc = XDocument.Parse(FileContents);

            //string[] lines = FileContents.Split(Environment.NewLine, StringSplitOptions.None);
            for (int i = 0; i < DocumentLines.Count; i++)
            {
                string line = DocumentLines[i];

                // In-line span splitting the line number and the contents of the line.
                Span xml_line = new();

                xml_line.Inlines.Add(new Run($"{i}: "));
                xml_line.Inlines.Add(new Run(line));
                //Paragraph xml_line = new();
                //xml_line.
                //XmlDocument.Blocks.Add(new Block())
                XmlDocument.Blocks.Add(new Paragraph(xml_line));
            }

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
