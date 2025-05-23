﻿using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TwUiEd.Core.Services;

namespace TwUiEd.Core.Models
{
    // The contents of an individual Twui file.
    public class TwuiFileModel
    {
        public string Name { get; private set; }
        public string FilePath { get; private set; }
        public string FileContents { get; private set; }

        public TwuiModel Model { get; private set; }

        public ObservableCollection<TwuiComponentModel> Root =>
        [
            Model.Root
        ];

        public TextDocument TextDocument { get; private set; }

        public TwuiFileModel(string filePath, string fileContents)
        {
            //Name = name;
            FilePath = filePath;
            FileContents = fileContents;

            // Grab the name of our file, everything after the final folder name. We'll also need to
            // create our TextDocument for the XML Editor view.
            Name = FilePath.Substring(FilePath.LastIndexOfAny(['/', '\\']));
            TextDocument = new TextDocument(FileContents);

            //XmlReader xmlReader = new XmlReader(FileContents);

            //Model = TwuiParser.ParseTwuiFile(fileContents);
            //var model = TwuiParserSerialized.ParseXmlFile(FilePath);
            //if (model != null)
            //{
            //    Model = model;
            //}

            TwuiParserSerialized.ParseXmlFile(FilePath);
        }
    }
}
