using SimpleMVVM;
using System;
using System.Windows.Input;

namespace Google
{
  public class AuthorizationPresenter : ObservableObject
  {
    private string accessCode;

    public string AccessCode
    {
      get
      {
        return this.accessCode;
      }

      set
      {
        this.accessCode = value;
        this.RaisePropertyChangedEvent("AccessCode");
      }
    }

    public ICommand AuthorizeCommand { get { return new DelegateCommand(this.Authorize); } }

    public event EventHandler PresenterClosed;

    public void OnPresenterClosed()
    {
      if (this.PresenterClosed != null)
        this.PresenterClosed(this, EventArgs.Empty);
    }

    private void Authorize(object parameter)
    {
      GoogleAuthorizationManager.InitializeSpreadsheetsService(GoogleAuthorizationManager.AuthorizationParameters, this.AccessCode);
      this.OnPresenterClosed();
    }


    public AuthorizationPresenter()
    {
      _deleteCustomerCommand = new DelegateCommand(DeleteCustomer, CanDeleteCustomer);
    }
    private readonly ICommand _deleteCustomerCommand;
    private bool CanDeleteCustomer(object state)
    {
      return true;
    }

    private void DeleteCustomer(object state)
    {
      
    }
  }
}
