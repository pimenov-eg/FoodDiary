using System;
using System.Windows.Input;
using SimpleMVVM;

namespace Google
{
  /// <summary>
  /// Презентер авторизации в Google.
  /// </summary>
  public class AuthorizationPresenter : ObservableObject
  {
    #region Поля и свойства

    private string accessCode;

    /// <summary>
    /// Код доступа.
    /// </summary>
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

    #endregion

    #region Методы

    /// <summary>
    /// Событие на закрытие презентера.
    /// </summary>
    public event EventHandler PresenterClosed;

    /// <summary>
    /// Генерация события на закрытие презентера.
    /// </summary>
    public void OnPresenterClosed()
    {
      if (this.PresenterClosed != null)
        this.PresenterClosed(this, EventArgs.Empty);
    }

    #endregion

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