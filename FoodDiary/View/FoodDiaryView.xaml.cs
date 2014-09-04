using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

    private void ComboBoxLoadedHandler(object sender, RoutedEventArgs e)
    {
      // http://www.codeproject.com/Questions/410084/Autocomplete-Combobox-in-WPF
      var comboBox = (ComboBox)sender;
      var textBox = comboBox.Template.FindName("PART_EditableTextBox", comboBox) as TextBox;
      var popup = comboBox.Template.FindName("PART_Popup", comboBox) as Popup;
      if (textBox != null)
      {
        textBox.TextChanged += delegate
        {
          comboBox.Items.Filter += (item) =>
          {
            if (item.ToString().StartsWith(textBox.Text))
            {
              popup.IsOpen = true;
              return true;

            }
            else
            {
              // popup.IsOpen = false;
              return false;
            }
          };

        };
      }
    }
  }
}