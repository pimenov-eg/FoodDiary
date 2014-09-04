using System.Collections.Generic;
using System.Globalization;
using FoodDiary.Interfaces;
using FoodDiary.Model;
using Google;
using Google.GData.Spreadsheets;

namespace FoodDiary
{
  /// <summary>
  /// Хранилище продуктов в Google Spreadsheet.
  /// </summary>
  public class GoogleProductStorage : IProductStorage
  {
    #region Поля и свойства

    /// <summary>
    /// Имя файла.
    /// </summary>
    private static readonly string SpreadSheetName = FoodDiary.Properties.Settings.Default.SpreadSheetName;

    /// <summary>
    /// Имя листа со списком продуктов.
    /// </summary>
    private static readonly string ProductsWorkSheetName = FoodDiary.Properties.Settings.Default.ProductsWorkSheetName;

    #endregion

    #region Методы

    /// <summary>
    /// Получить все продукты.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Product> GetAllProducts()
    {
      ListFeed listFeed = SpreadsheetsManager.GetRows(SpreadSheetName, ProductsWorkSheetName);
      if (listFeed == null)
        return null;

      var allProducts = new List<Product>();
      foreach (ListEntry row in listFeed.Entries)
      {
        var product = new Product();
        foreach (ListEntry.Custom element in row.Elements)
        {
          switch (element.LocalName)
          {
            case "product":
              product.Name = element.Value;
              break;
            case "calvalue":
              product.CalValue = float.Parse(element.Value, CultureInfo.InvariantCulture.NumberFormat);
              break;
            case "protein":
              product.Protein = float.Parse(element.Value, CultureInfo.InvariantCulture.NumberFormat);
              break;
            case "carbohydrate":
              product.Carbohydrate = float.Parse(element.Value, CultureInfo.InvariantCulture.NumberFormat);
              break;
            case "fat":
              product.Fat = float.Parse(element.Value, CultureInfo.InvariantCulture.NumberFormat);
              break;
            default:
              break;
          }

        }
        allProducts.Add(product);
      }

      ProductCache.AllProducts = allProducts;
      return allProducts;
    }

    /// <summary>
    /// Добавить продукт в хранилище.
    /// </summary>
    /// <param name="product">Продукт.</param>
    public void AddProduct(Product product)
    {
      ListFeed listFeed = SpreadsheetsManager.GetRows(SpreadSheetName, ProductsWorkSheetName);
      if (listFeed == null)
        return;

      var rowValue = new Dictionary<string, string>
      {
        {"product", product.Name},
        {"calvalue", product.CalValue.ToString("N", CultureInfo.InvariantCulture)},
        {"protein", product.Protein.ToString("N", CultureInfo.InvariantCulture)},
        {"carbohydrate", product.Carbohydrate.ToString("N", CultureInfo.InvariantCulture)},
        {"fat", product.Fat.ToString("N", CultureInfo.InvariantCulture)}
      };
      SpreadsheetsManager.AddNewRow(listFeed, rowValue);
    }

    #endregion
  }
}