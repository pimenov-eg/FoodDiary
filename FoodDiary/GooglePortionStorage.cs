﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FoodDiary.Interfaces;
using FoodDiary.Model;
using Google;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace FoodDiary
{
  /// <summary>
  /// Хранилище рациона в Google Spreadsheet.
  /// </summary>
  public class GooglePortionStorage : IPortionStorage
  {
    #region Поля и свойства

    /// <summary>
    /// Заголовки колонок по умолчанию.
    /// </summary>
    private static readonly string[] DefaultColumnHeaders = new string[]
    {
      "Date", "Product", "Weight", "Protein", "Carbohydrate", "Fat", "CalValue",
      "CalResult", "ProteinResult", "CarbohydrateResult", "FatResult"
    };

    /// <summary>
    /// Имя файла.
    /// </summary>
    private static readonly string SpreadSheetName = FoodDiary.Properties.Settings.Default.SpreadSheetName;

    /// <summary>
    /// Имя листа с рационом за месяц.
    /// </summary>
    private static readonly string MonthWorkSheetName = FoodDiary.Properties.Settings.Default.MonthWorkSheetName;

    /// <summary>
    /// Номер текущего месяца.
    /// </summary>
    private static readonly int CurrentMonthNumber = DateTime.Now.Month;

    /// <summary>
    /// Имя листа с рационом за текущий месяц.
    /// </summary>
    private readonly string CurrentMonthWorkSheetName = string.Join("-", MonthWorkSheetName, CurrentMonthNumber);

    #endregion

    #region Методы

    /// <summary>
    /// Сохранить дневной рацион.
    /// </summary>
    /// <param name="dailyPortion">Дневной рацион.</param>
    public void SaveDailyPortion(DailyPortion dailyPortion)
    {
      ListFeed listFeed = SpreadsheetsManager.GetRows(SpreadSheetName, CurrentMonthWorkSheetName);
      if (listFeed == null)
        return;

      if (this.HasCurrentDailyPortion(listFeed))
        this.DeleteCurrentDailyPortion();
      this.AddNewCurrentDailyPortion(dailyPortion, listFeed);
    }

    /// <summary>
    /// Определить есть ли уже записи о текущем дневном рационе.
    /// </summary>
    /// <param name="listFeed">Источник.</param>
    /// <returns>True - если записи о текущем дневном рационе есть.</returns>
    private bool HasCurrentDailyPortion(ListFeed listFeed)
    {
      string currentDate = DateTime.Now.ToShortDateString();
      foreach (ListEntry row in listFeed.Entries)
      {
        foreach (ListEntry.Custom element in row.Elements)
        {
          if (element.LocalName == "date" && element.Value == currentDate)
            return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Удалить текущий дневной рацион.
    /// </summary>
    private void DeleteCurrentDailyPortion()
    {
      // Удаление через ListBased-нотацию работает криво, т.к. после каждого удаления обновляется коллекция и следующее удаление "пропускает" одну запись.
      CellQuery cellQuery = SpreadsheetsManager.InitializeCellBasedQuery(SpreadSheetName, CurrentMonthWorkSheetName);
      CellFeed cellFeed = SpreadsheetsManager.Service.Query(cellQuery);

      string currentDate = DateTime.Now.ToShortDateString();
      var firstRowForCurrentDay = cellFeed.Entries.OfType<CellEntry>().Where(e => e.Value == currentDate).Min(e => e.Row);
      var lastRowForCurrentDay = cellFeed.Entries.OfType<CellEntry>().Where(e => e.Value == currentDate).Max(e => e.Row);
      var cellsForCurrentDay = cellFeed.Entries.OfType<CellEntry>().Where(c => c.Row >= firstRowForCurrentDay && c.Row <= lastRowForCurrentDay).ToList();
      CellFeed batchRequest = new CellFeed(cellQuery.Uri, SpreadsheetsManager.Service);
      foreach (CellEntry cell in cellsForCurrentDay)
      {
        cell.InputValue = string.Empty;
        cell.BatchData = new GDataBatchEntryData(GDataBatchOperationType.update);
        batchRequest.Entries.Add(cell);
      }

      CellFeed batchResponse = (CellFeed)SpreadsheetsManager.Service.Batch(batchRequest, new Uri(cellFeed.Batch));
    }

    /// <summary>
    /// Добавить новый рацион за день.
    /// </summary>
    /// <param name="dailyPortion">Рацион за день.</param>
    /// <param name="listFeed">Источник.</param>
    private void AddNewCurrentDailyPortion(DailyPortion dailyPortion, ListFeed listFeed)
    {
      var collapsedEatingProducts = dailyPortion.AllEatingProducts.GroupBy(p => p.Product.Name, (name, products) => new OneTimePortion
      {
        Weight = products.Sum(p => p.Weight),
        Product = new Product
        {
          Name = name,
          CalValue = products.Sum(p => p.Product.CalValue),
          Carbohydrate = products.Sum(p => p.Product.Carbohydrate),
          Fat = products.Sum(p => p.Product.Fat),
          Protein = products.Sum(p => p.Product.Protein),
        }
      });

      foreach (var oneTimePortion in collapsedEatingProducts)
      {
        var totalProtein = (oneTimePortion.Product.Protein / 100) * oneTimePortion.Weight;
        var totalCarbohydrate = (oneTimePortion.Product.Carbohydrate / 100) * oneTimePortion.Weight;
        var totalFat = (oneTimePortion.Product.Fat / 100) * oneTimePortion.Weight;
        var totalCalValue = (oneTimePortion.Product.CalValue / 100) * oneTimePortion.Weight;

        var rowValue = new Dictionary<string, string>
        {
          {"date", dailyPortion.Date},
          {"product", oneTimePortion.Product.Name},
          {"weight", oneTimePortion.Weight.ToString("N", CultureInfo.InvariantCulture)},
          {"protein", totalProtein.ToString("N", CultureInfo.InvariantCulture)},
          {"carbohydrate", totalCarbohydrate.ToString("N", CultureInfo.InvariantCulture)},
          {"fat", totalFat.ToString("N", CultureInfo.InvariantCulture)},
          {"calvalue", totalCalValue.ToString("N", CultureInfo.InvariantCulture)}
        };
        SpreadsheetsManager.AddNewRow(listFeed, rowValue);
      }

      var resultRowValue = new Dictionary<string, string>
      {
        {"date", dailyPortion.Date},
        {"calresult", dailyPortion.TotalCalValue.ToString("N", CultureInfo.InvariantCulture)},
        {"proteinresult", dailyPortion.TotalProteinValue.ToString("N", CultureInfo.InvariantCulture)},
        {"carbohydrateresult", dailyPortion.TotalCarbohydrateValue.ToString("N", CultureInfo.InvariantCulture)},
        {"fatresult", dailyPortion.TotalFatValue.ToString("N", CultureInfo.InvariantCulture)}
      };
      SpreadsheetsManager.AddNewRow(listFeed, resultRowValue);
    }

    /// <summary>
    /// Получить текущий дневной рацион.
    /// </summary>
    /// <returns>Дневной рацион.</returns>
    public DailyPortion GetCurrentDailyPortion()
    {
      ListFeed listFeed = SpreadsheetsManager.GetRows(SpreadSheetName, CurrentMonthWorkSheetName, DefaultColumnHeaders);
      if (listFeed == null)
        return null;

      var currentDailyPortion = new DailyPortion();
      bool isCurrentDate = false;
      bool needIterate = true;
      string currentDate = DateTime.Now.ToShortDateString();
      foreach (ListEntry row in listFeed.Entries)
      {
        if (!needIterate)
          break;

        var oneTimePortion = new OneTimePortion();
        foreach (ListEntry.Custom element in row.Elements)
        {
          switch (element.LocalName)
          {
            case "date":
              if (currentDate == element.Value)
              {
                isCurrentDate = true;
                currentDailyPortion.Date = element.Value;
              }
              else if (string.IsNullOrEmpty(element.Value))
              {
                needIterate = false;
              }
              break;
            case "product":
              if (isCurrentDate && !string.IsNullOrEmpty(element.Value))
              {
                oneTimePortion.Product = ProductCache.AllProducts.First(p => p.Name == element.Value);
              }
              break;
            case "weight":
              if (isCurrentDate && !string.IsNullOrEmpty(element.Value))
              {
                oneTimePortion.Weight = float.Parse(element.Value, CultureInfo.InvariantCulture.NumberFormat);
              }
              break;
            default:
              break;
          }
        }
        if (oneTimePortion.Product != null)
          currentDailyPortion.AllEatingProducts.Add(oneTimePortion);
      }
      return currentDailyPortion;
    }

    #endregion
  }
}