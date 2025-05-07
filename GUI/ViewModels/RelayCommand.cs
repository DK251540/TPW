using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BilardApp.GUI.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Func<object, Task> _execute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Func<object, Task> execute) => _execute = execute;

        public bool CanExecute(object parameter) => true;

        public async void Execute(object parameter) => await _execute(parameter);
    }
}
