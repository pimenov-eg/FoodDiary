using System;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace Google
{
  /// <summary>
  /// Менеджер авторизации Google.
  /// </summary>
  public static class GoogleAuthorizationManager
  {
    #region Поля и свойства

    /// <summary>
    /// Параметры авторизации.
    /// </summary>
    public static OAuth2Parameters AuthorizationParameters;

    #endregion

    #region Методы

    /// <summary>
    /// Получить строку авторизации.
    /// </summary>
    /// <returns>Строка авторизации.</returns>
    public static string GetAuthorizationUrl()
    {
      string CLIENT_ID = Google.Properties.Settings.Default.ClientId;
      string CLIENT_SECRET = Google.Properties.Settings.Default.ClientSecret;
      // Space separated list of scopes for which to request access.
      string SCOPE = "https://spreadsheets.google.com/feeds https://docs.google.com/feeds";
      string REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";

      AuthorizationParameters = new OAuth2Parameters()
      {
        ClientId = CLIENT_ID,
        ClientSecret = CLIENT_SECRET,
        Scope = SCOPE,
        RedirectUri = REDIRECT_URI
      };

      return OAuthUtil.CreateOAuth2AuthorizationUrl(AuthorizationParameters);
    }

    /// <summary>
    /// Инициализировать сервис для работы с Google-таблицами.
    /// </summary>
    /// <param name="parameters">Параметры авторизации.</param>
    /// <param name="accessCode">Код подтверждения.</param>
    /// <returns>Инициализированный экземпляр сервиса для работы с Google-таблицами.</returns>
    public static SpreadsheetsService InitializeSpreadsheetsService(OAuth2Parameters parameters, string accessCode)
    {
      parameters.AccessCode = accessCode;
      OAuthUtil.GetAccessToken(parameters);

      // Initialize the variables needed to make the request
      var requestFactory = new GOAuth2RequestFactory(null, "Google", parameters);
      SpreadsheetsManager.Service = new SpreadsheetsService("Google");
      SpreadsheetsManager.Service.RequestFactory = requestFactory;

      OnSpreadsheetsServiceInitialized();

      return SpreadsheetsManager.Service;
    }

    /// <summary>
    /// Событие на завершение иницилазиации сервиса Google Spreadsheet.
    /// </summary>
    public static event EventHandler SpreadsheetsServiceInitialized;

    /// <summary>
    /// Генерация события на завершение инициализации сервиса Google Spreadsheet.
    /// </summary>
    public static void OnSpreadsheetsServiceInitialized()
    {
      if (SpreadsheetsServiceInitialized != null)
        SpreadsheetsServiceInitialized(null, EventArgs.Empty);
    }

    #endregion
  }
}