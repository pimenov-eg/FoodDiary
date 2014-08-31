﻿namespace FoodDiary.Model
{
  /// <summary>
  /// Разовый прием пищи.
  /// </summary>
  public class OneTimePortion
  {
    /// <summary>
    /// Продукт.
    /// </summary>
    public Product Product { get; set;}

    /// <summary>
    /// Потребленная масса в граммах.
    /// </summary>
    public float Weight { get; set; }

    public override string ToString()
    {
      return string.Format("{0}, {1} гр.", this.Product.Name, this.Weight);
    }
  }
}
