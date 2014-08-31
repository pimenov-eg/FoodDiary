using Google.GData.Client;
using Google.GData.Spreadsheets;
using System;
using System.Configuration;
using System.Collections.Specialized;

namespace Google
{
  /// <summary>
  /// Менеджер авторизации Google.
  /// </summary>
  public static class GoogleAuthorizationManager
  {
    /// <summary>
    /// Параметры авторизации.
    /// </summary>
    public static OAuth2Parameters AuthorizationParameters;

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
      var requestFactory = new GOAuth2RequestFactory(null, "FoodDiary", parameters);
      SpreadsheetsManager.Service = new SpreadsheetsService("FoodDiary");
      SpreadsheetsManager.Service.RequestFactory = requestFactory;

      OnSpreadsheetsServiceInitialized();

      return SpreadsheetsManager.Service;
    }

    public static event EventHandler SpreadsheetsServiceInitialized;

    public static void OnSpreadsheetsServiceInitialized()
    {
      if (SpreadsheetsServiceInitialized != null)
        SpreadsheetsServiceInitialized(null, EventArgs.Empty);
    }
  }
}
