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
    #region 
    /// <summary>
    /// Количество столбцов по умолчанию.
    /// </summary>
    public const int DefaultColumnCount = 15;

    /// <summary>
    /// Количество строк по умолчанию.
    /// </summary>
    public const int DefaultRowCount = 15;

    #endregion

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
      if (SpreadsheetsManager.Service == null)
        return null;

      var spreadsheet = GetSpreadSheet(spreadsheetName);
      if (spreadsheet == null)
        return null;

      var worksheet = GetWorkSheet(spreadsheet, worksheetName);
      if (worksheet == null)
        worksheet = CreateNewWorkSheet(spreadsheet, worksheetName);

      // Define the URL to request the list feed of the worksheet.
      var listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
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
      if (SpreadsheetsManager.Service == null)
        return null;

      var spreadsheet = GetSpreadSheet(spreadsheetName);
      if (spreadsheet == null)
        return null;

      var worksheet = GetWorkSheet(spreadsheet, worksheetName);
      if (worksheet == null)
        worksheet = CreateNewWorkSheet(spreadsheet, worksheetName);

      // Fetch the cell feed of the worksheet.
      return new CellQuery(worksheet.CellFeedLink);
    }

    private static SpreadsheetEntry GetSpreadSheet(string spreadsheetName)
    {
      // Instantiate a SpreadsheetQuery object to retrieve spreadsheets.
      var query = new SpreadsheetQuery();

      return SpreadsheetsManager.Service.Query(query).Entries
        .OfType<SpreadsheetEntry>()
        .Where(e => e.Title.Text == spreadsheetName)
        .FirstOrDefault();
    }

    private static WorksheetEntry GetWorkSheet(SpreadsheetEntry spreadsheet, string worksheetName)
    {
      return spreadsheet.Worksheets.Entries
        .OfType<WorksheetEntry>()
        .Where(w => w.Title.Text == worksheetName)
        .FirstOrDefault();
    }

    private static WorksheetEntry CreateNewWorkSheet(SpreadsheetEntry spreadsheet, string worksheetName)
    {
      // Create a local representation of the new worksheet.
      var worksheet = new WorksheetEntry();
      worksheet.Title.Text = worksheetName;
      worksheet.Cols = DefaultColumnCount;
      worksheet.Rows = DefaultRowCount;

      // Send the local representation of the worksheet to the API for
      // creation.  The URL to use here is the worksheet feed URL of our
      // spreadsheet.
      WorksheetFeed wsFeed = spreadsheet.Worksheets;
      SpreadsheetsManager.Service.Insert(wsFeed, worksheet);

      var reloadedSpreadsheet = GetSpreadSheet(spreadsheet.Title.Text);
      return GetWorkSheet(reloadedSpreadsheet, worksheetName);
    }
  }
}