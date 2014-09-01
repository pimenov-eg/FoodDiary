using FoodDiary.Interfaces;
using FoodDiary.Model;
using Google;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FoodDiary
{
  public class GoogleProductStorage : IProductStorage
  {
    /// <summary>
    /// Имя файла.
    /// </summary>
    private static readonly string SpreadSheetName = FoodDiary.Properties.Settings.Default.SpreadSheetName;

    /// <summary>
    /// Имя листа со списком продуктов.
    /// </summary>
    private static readonly string ProductsWorkSheetName = FoodDiary.Properties.Settings.Default.ProductsWorkSheetName;

    public IEnumerable<Product> GetAllProducts()
    {
      ListFeed listFeed = SpreadsheetsManager.GetRows(SpreadSheetName, ProductsWorkSheetName);
      if (listFeed == null)
        return null;

      List<Product> allProducts = new List<Product>();
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

    public void AddProduct(Product product)
    {
      ListFeed listFeed = SpreadsheetsManager.GetRows(SpreadSheetName, ProductsWorkSheetName);
      if (listFeed == null)
        return;

      var row = new ListEntry();
      row.Elements.Add(new ListEntry.Custom() { LocalName = "product", Value = product.Name });
      row.Elements.Add(new ListEntry.Custom() { LocalName = "calvalue", Value = product.CalValue.ToString() });
      row.Elements.Add(new ListEntry.Custom() { LocalName = "protein", Value = product.Protein.ToString() });
      row.Elements.Add(new ListEntry.Custom() { LocalName = "carbohydrate", Value = product.Carbohydrate.ToString() });
      row.Elements.Add(new ListEntry.Custom() { LocalName = "fat", Value = product.Fat.ToString() });

      SpreadsheetsManager.Service.Insert(listFeed, row);
    }
  }
}
