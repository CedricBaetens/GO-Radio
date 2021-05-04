using System;
using System.Windows.Input;

namespace GoRadio.DesktopApp.ViewModel
{
    public class Command<T> : ICommand
    {
        public T Data { get; set; }

        private readonly Action<T> _action;

        public event EventHandler CanExecuteChanged;

        public Command(T data, Action<T> action)
        {
            Data = data;
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action(Data);
        }
    }
}
