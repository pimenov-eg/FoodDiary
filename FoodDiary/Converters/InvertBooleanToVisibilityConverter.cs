using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FoodDiary
{
  /// <summary>
  /// Конвертер противоположности булевского значения в значение видимости.
  /// </summary>
  [ValueConversion(typeof(bool), typeof(Visibility))]
  public class InvertBooleanToVisibilityConverter : IValueConverter
  {
    /// <summary>
    /// Конвертер булевского значения в значение видимости.
    /// </summary>
    private static readonly BooleanToVisibilityConverter booleanToVisibilityConverter = new BooleanToVisibilityConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return booleanToVisibilityConverter.Convert(!(bool)value, targetType, parameter, culture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return !(bool)booleanToVisibilityConverter.Convert(value, targetType, parameter, culture);
    }
  }
}