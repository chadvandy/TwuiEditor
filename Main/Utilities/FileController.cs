using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// TODO the internal file controller system

namespace Main.Utilities
{
    internal class FileController
    {
        // TODO opened files;
            // TODO abstract to another class!
        List<string> filePaths;

        private string openFilePath;

        public string OpenFilePath { get => openFilePath; }



        // TODO open new file
        string TryOpen(string filePath)
        {
            return "";
        }
    }
}
