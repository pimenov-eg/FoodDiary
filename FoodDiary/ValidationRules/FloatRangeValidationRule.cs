﻿using System;
using System.Globalization;
using System.Windows.Controls;

namespace FoodDiary
{
  /// <summary>
  /// Правило валидации на проверку вхождения в заданный интервал чисел.
  /// </summary>
  public class FloatRangeValidationRule : ValidationRule
  {
    #region Константы

    /// <summary>
    /// Значение по умолчанию для минимального значения.
    /// </summary>
    public const float DefaultMinValue = 0;

    /// <summary>
    /// Значение по умолчанию для максимального значения.
    /// </summary>
    public const float DefaultMaxValue = 1000;

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Минимальное значение.
    /// </summary>
    public float MinValue { get; set; }

    /// <summary>
    /// Максимальное значение.
    /// </summary>
    public float MaxValue { get; set; }

    #endregion

    #region Методы

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
      float convertedValue = 0;

      try
      {
        if (((string)value).Length > 0)
          convertedValue = float.Parse((String)value, CultureInfo.InvariantCulture.NumberFormat);
      }
      catch
      {
        return new ValidationResult(false, "Incorrect format.");
      }

      if (convertedValue < this.MinValue || convertedValue > this.MaxValue)
        return new ValidationResult(false, "Value mast be in the range: " + MinValue + " - " + MaxValue + ".");
      else
        return new ValidationResult(true, null);
    }

    #endregion

    #region Конструкторы

    public FloatRangeValidationRule()
    {
      this.MinValue = DefaultMinValue;
      this.MaxValue = DefaultMaxValue;
    }

    #endregion
  }
}