using FoodDiary.View;
using Google;
using System.Windows;

namespace FoodDiary
{
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void HyperlinkClickHandler(object sender, RoutedEventArgs e)
    {
      new AuthorizationWindow().ShowDialog();
    }
  }
}
