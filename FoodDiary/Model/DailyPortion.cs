using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodDiary.Model
{
  /// <summary>
  /// Дневной рацион.
  /// </summary>
  public class DailyPortion
  {
    /// <summary>
    /// Дата.
    /// </summary>
    public string Date { get; set; }

    /// <summary>
    /// Все потребленные продукты.
    /// </summary>
    public IList<OneTimePortion> AllEatingProducts { get; set; }

    /// <summary>
    /// Общее количество поребленных килокалорий.
    /// </summary>
    public float TotalCalValue { get { return this.AllEatingProducts.Sum(p => (p.Product.CalValue / 100) * p.Weight); } }

    /// <summary>
    /// Общее количество потребленных белков.
    /// </summary>
    public float TotalProteinValue { get { return this.AllEatingProducts.Sum(p => (p.Product.Protein / 100) * p.Weight); } }

    /// <summary>
    /// Общее количество потребленных углеводов.
    /// </summary>
    public float TotalCarbohydrateValue { get { return this.AllEatingProducts.Sum(p => (p.Product.Carbohydrate / 100) * p.Weight); } }

    /// <summary>
    /// Общее количество потребленных жиров.
    /// </summary>
    public float TotalFatValue { get { return this.AllEatingProducts.Sum(p => (p.Product.Fat / 100) * p.Weight); } }

    public DailyPortion()
    {
      this.AllEatingProducts = new List<OneTimePortion>();
    }
  }
}
