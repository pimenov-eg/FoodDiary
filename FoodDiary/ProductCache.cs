using FoodDiary.Model;
using System.Collections.Generic;
using System.Linq;

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
    public static IList<Product> AllProducts { get; set; }

    /// <summary>
    /// Проверить наличие продукта в кэше.
    /// </summary>
    /// <param name="product">Продукт.</param>
    /// <returns>True - если продукт в кэше есть.</returns>
    public static bool ContainsProduct(Product product)
    {
      return AllProducts.Any(p => p.Name == product.Name);
    }
  }
}