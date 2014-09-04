using System.Collections.Generic;
using FoodDiary.Model;

namespace FoodDiary
{
  /// <summary>
  /// Кэш продуктов.
  /// </summary>
  public static class ProductCache
  {
    /// <summary>
    /// Все продукты.
    /// </summary>
    public static IEnumerable<Product> AllProducts { get; set; }
  }
}