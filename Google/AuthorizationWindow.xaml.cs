using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Google
{
  /// <summary>
  /// Interaction logic for AuthorizationWindow.xaml
  /// </summary>
  public partial class AuthorizationWindow : Window
  {
    private readonly AuthorizationPresenter presenter = new AuthorizationPresenter();

    public AuthorizationWindow()
    {
      InitializeComponent();
      this.DataContext = presenter;
      this.presenter.PresenterClosed += (s, e) => this.Close();
    }

    private void WebBrowserSourceUpdatedHanlder(object sender, DataTransferEventArgs e)
    {
      var webBrowser = (WebBrowser)sender;
      if (webBrowser.Source != null)
        webBrowser.Navigate(webBrowser.Source);
    }

    private void WindowLoadedHandler(object sender, RoutedEventArgs e)
    {
      var authorizationUrl = GoogleAuthorizationManager.GetAuthorizationUrl();
      this.webBrowser.Source = new Uri(authorizationUrl);
    }
  }
}
