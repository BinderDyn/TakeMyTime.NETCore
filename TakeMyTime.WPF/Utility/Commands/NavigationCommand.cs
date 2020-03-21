using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace TakeMyTime.WPF.Utility.Commands
{
    public class NavigationCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly Action _action;
        public NavigationCommand(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
