using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using TwUiEd.Core.Models;

namespace TwUiEd.Core.Services
{
    public interface IFileService
    {
        public TwuiFileModel OpenFile(string filePath, string fileContents);
    }
     
    // Our FileService, that handles opening, closing, saving, etc., files.
    // It also holds our currently held files.
    public class FileService : IFileService
    {
        public TwuiFileModel OpenFile(string filePath, string fileContents)
        {
            return new(filePath, fileContents);
        }
    }
}
