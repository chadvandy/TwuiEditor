using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GroovyCommon.Abstractions.ViewModels
{
    public abstract class ViewModelBase : ObservableObject, IDisposable
    {
        public string DisplayName { get; protected set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
