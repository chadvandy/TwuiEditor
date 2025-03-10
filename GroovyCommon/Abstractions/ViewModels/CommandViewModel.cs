using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroovyCommon.Abstractions.ViewModels
{
    public class CommandViewModel : ViewModelBase
    {
        public RelayCommand Command { get; private set; }

        //public override string DisplayName { get; set; }
        public CommandViewModel(string displayName, RelayCommand command)
        {
            Command = command;

            this.DisplayName = displayName;
        }
    }
}
