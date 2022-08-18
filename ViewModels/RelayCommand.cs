using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace EnvControlPanel.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action action;
        private readonly Func<bool> canExecute;

        public RelayCommand(Action action)
            : this(action, null)
        {
        }

        public RelayCommand(Action action, Func<bool> canExecute)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            this.action = action;
            this.canExecute = canExecute;
        }

        

        public bool CanExecute(object parameter) => canExecute == null || canExecute();

        public void Execute(object parameter) => action();

        public event EventHandler<object> CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);




        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
              
            }

            remove
            {
                
            }
        }

    }
}
