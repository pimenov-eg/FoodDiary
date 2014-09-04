using System;
using System.Globalization;
using System.Windows.Data;

namespace FoodDiary
{
  /// <summary>
  /// Конвертер булевского значения в противоположное.
  /// </summary>
  [ValueConversion(typeof(bool), typeof(bool))]
  public class InvertBooleanConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return !(bool)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return Binding.DoNothing;
    }
  }
}