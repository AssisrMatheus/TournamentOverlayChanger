using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OverlayChanger.Classes
{
    public class Command : ICommand
    {
#pragma warning disable
        public event EventHandler CanExecuteChanged;
#pragma warning enable

        private Action<object> execute;

        public Command(Action<object> execute)
        {
            this.execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            execute?.Invoke(parameter);
        }
    }
}
