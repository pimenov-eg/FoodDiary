using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FoodDiary
{
  public class WeightValidationRule : ValidationRule
  {
    public int MinWeight { get; set; }

    public int MaxWeight { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
      int weight = 0;

      try
      {
        if (((string)value).Length > 0)
          weight = Int32.Parse((String)value);
      }
      catch (Exception e)
      {
        return new ValidationResult(false, "Illegal characters or " + e.Message);
      }

      if (weight < this.MinWeight || weight > this.MaxWeight)
        return new ValidationResult(false, "Please enter weight value in the range: " + MinWeight + " - " + MaxWeight + ".");
      else
        return new ValidationResult(true, null);
    }

    public WeightValidationRule() { }
  }
}