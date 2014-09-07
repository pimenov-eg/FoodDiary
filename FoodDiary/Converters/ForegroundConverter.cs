using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FoodDiary
{
  /// <summary>
  /// Конвертер двух входных чисел в цвет.
  /// </summary>
  public class ForegroundConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      var ccalValue = float.Parse((string)values[0]);
      var goalCcalValue = float.Parse((string)values[1]);
      bool valueIsNotFilled = ccalValue == 0;
      
      if (valueIsNotFilled)
        return Brushes.Black;
      return ccalValue < goalCcalValue ? Brushes.Red : Brushes.Green;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      return null;
    }
  }
}