using Google.GData.Client;
using Google.GData.Spreadsheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google
{
  public class SpreadsheetsManager
  {
    public static SpreadsheetsService Service { get; set; }

    /// <summary>
    /// Получить все строки старницы.
    /// </summary>
    /// <param name="spreadsheetName">Имя книги (файла).</param>
    /// <param name="worksheetName">Имя страницы.</param>
    /// <returns>Все строки страницы.</returns>
    public static ListFeed GetRows(string spreadsheetName, string worksheetName)
    {
      ListQuery listQuery = InitializeListBasedQuery(spreadsheetName, worksheetName);
      return SpreadsheetsManager.Service.Query(listQuery);
    }

    /// <summary>
    /// Получить все ячейки старницы.
    /// </summary>
    /// <param name="spreadsheetName">Имя книги (файла).</param>
    /// <param name="worksheetName">Имя страницы.</param>
    /// <returns>Все ячейки страницы.</returns>
    public static CellFeed GetCells(string spreadsheetName, string worksheetName)
    {
      CellQuery cellQuery = InitializeCellBasedQuery(spreadsheetName, worksheetName);
      return SpreadsheetsManager.Service.Query(cellQuery);
    }

    /// <summary>
    /// Получить все строки старницы по заданному фильтру.
    /// </summary>
    /// <param name="spreadsheetName">Имя книги (файла).</param>
    /// <param name="worksheetName">Имя страницы.</param>
    /// <param name="filter">Фильтр.</param>
    /// <returns>Все строки страницы по заданному фильтру.</returns>
    public static ListFeed GetRowsWithFilter(string spreadsheetName, string worksheetName, string filter)
    {
      ListQuery listQuery = InitializeListBasedQuery(spreadsheetName, worksheetName);
      if (!string.IsNullOrEmpty(filter))
      { 
        listQuery.SpreadsheetQuery = filter;
        listQuery.Reverse = true;
      }
      return SpreadsheetsManager.Service.Query(listQuery);
    }

    /// <summary>
    /// Инициализировать запрос к сервису.
    /// </summary>
    /// <param name="spreadsheetName">Имя книги (файла).</param>
    /// <param name="worksheetName">Имя страницы.</param>
    /// <returns>Запрос к сервису.</returns>
    private static ListQuery InitializeListBasedQuery(string spreadsheetName, string worksheetName)
    {
      if (Service == null)
        return null;

      // Instantiate a SpreadsheetQuery object to retrieve spreadsheets.
      var query = new SpreadsheetQuery();

      var spreadsheet = SpreadsheetsManager.Service.Query(query).Entries
        .OfType<SpreadsheetEntry>()
        .Where(e => e.Title.Text == spreadsheetName)
        .FirstOrDefault();

      if (spreadsheet == null)
        return null;

      var productsWorksheet = spreadsheet.Worksheets.Entries
        .OfType<WorksheetEntry>()
        .Where(w => w.Title.Text == worksheetName)
        .FirstOrDefault();

      // Define the URL to request the list feed of the worksheet.
      var listFeedLink = productsWorksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
      // Fetch the list feed of the worksheet.
      return new ListQuery(listFeedLink.HRef.ToString());
    }

    /// <summary>
    /// Инициализировать запрос к сервису.
    /// </summary>
    /// <param name="spreadsheetName">Имя книги (файла).</param>
    /// <param name="worksheetName">Имя страницы.</param>
    /// <returns>Запрос к сервису.</returns>
    public static CellQuery InitializeCellBasedQuery(string spreadsheetName, string worksheetName)
    {
      if (Service == null)
        return null;

      // Instantiate a SpreadsheetQuery object to retrieve spreadsheets.
      var query = new SpreadsheetQuery();

      var spreadsheet = SpreadsheetsManager.Service.Query(query).Entries
        .OfType<SpreadsheetEntry>()
        .Where(e => e.Title.Text == spreadsheetName)
        .FirstOrDefault();

      if (spreadsheet == null)
        return null;

      var productsWorksheet = spreadsheet.Worksheets.Entries
        .OfType<WorksheetEntry>()
        .Where(w => w.Title.Text == worksheetName)
        .FirstOrDefault();

      // Fetch the cell feed of the worksheet.
      return new CellQuery(productsWorksheet.CellFeedLink);
    }
  }
}