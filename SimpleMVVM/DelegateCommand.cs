using System;
using System.Windows.Input;

namespace SimpleMVVM
{
  public class DelegateCommand : ICommand
  {
    private readonly Action action;

    public DelegateCommand(Action action)
    {
      this.action = action;
    }

    public void Execute(object parameter)
    {
      this.action();
    }

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public event EventHandler CanExecuteChanged;
  }
}
