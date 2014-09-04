using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Google;

namespace FoodDiary.View
{
  /// <summary>
  /// Interaction logic for FoodDiaryView.xaml
  /// </summary>
  public partial class FoodDiaryView : UserControl
  {
    public FoodDiaryView()
    {
      InitializeComponent();
    }

    private void HyperlinkClickHandler(object sender, RoutedEventArgs e)
    {
      new AuthorizationWindow().ShowDialog();
    }
  }
}
