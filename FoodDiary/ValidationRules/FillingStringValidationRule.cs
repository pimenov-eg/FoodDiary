using System.Globalization;
using System.Windows.Controls;

namespace FoodDiary
{
  /// <summary>
  /// Правило валидации на проверку заполненности строки.
  /// </summary>
  public class FillingStringValidationRule : ValidationRule
  {
    #region Методы

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
      string stringValue = string.Empty;

      try
      {
        stringValue = (string)value;
      }
      catch
      {
        return new ValidationResult(false, "Incorrect format.");
      }

      if (string.IsNullOrWhiteSpace(stringValue))
        return new ValidationResult(false, "Value mast be a string.");
      else
        return new ValidationResult(true, null);
    }

    #endregion

    #region Конструкторы

    public FillingStringValidationRule()
    {
    }

    #endregion
  }
}