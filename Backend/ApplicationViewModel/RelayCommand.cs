using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Backend.ApplicationViewModel
{
    public class RelayCommand : ICommand
    {
        private Action<object> functionToExecute;
        private Func<object, bool> canFunctionExecute;

        public RelayCommand(Action<object> functionToExecute, Func<object, bool> canExecute = null)
        {
            this.functionToExecute = functionToExecute;
            canFunctionExecute = canExecute;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canFunctionExecute == null || canFunctionExecute(parameter);
        }

        public void Execute(object parameter)
        {
            functionToExecute(parameter);
        }
    }
}