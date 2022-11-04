using System;
using System.Windows.Input;

namespace Emma.ViewModels.Commands
{
    public class ButtonPressedCommand : ICommand
    {
        private Action _execute;

        public ButtonPressedCommand(Action execute)
        {
            _execute = execute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _execute.Invoke();
        }
    }
}