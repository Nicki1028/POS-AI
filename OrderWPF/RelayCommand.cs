using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderWPF
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        Action callback;
        public RelayCommand(Action callback)
        {
            this.callback = callback;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            this.callback();
        }
    }

    public class RelayCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        Action<T> callback;
        public RelayCommand(Action<T> callback)
        {
            this.callback = callback;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            this.callback((T)parameter);
        }
    }
}
