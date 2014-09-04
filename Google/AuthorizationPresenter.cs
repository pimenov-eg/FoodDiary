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

    public event EventHandler PresenterClosed;

    public void OnPresenterClosed()
    {
      if (this.PresenterClosed != null)
        this.PresenterClosed(this, EventArgs.Empty);
    }

    #region Команда авторизации на сервисе Google Spreadsheet

    public ICommand AuthorizeCommand { get { return new DelegateCommand(this.Authorize, this.CanAuthorize); } }

    private void Authorize(object parameter)
    {
      GoogleAuthorizationManager.InitializeSpreadsheetsService(GoogleAuthorizationManager.AuthorizationParameters, this.AccessCode);
      this.OnPresenterClosed();
    }

    private bool CanAuthorize(object parameter)
    {
      return true;
    }

    #endregion

    #region Конструкторы

    public AuthorizationPresenter()
    {
    }

    #endregion
  }
}
