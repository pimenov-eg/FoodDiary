using System.Collections.Generic;
using System.Linq;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace Google
{
  /// <summary>
  /// Менеджер для работы с Google Spreadsheet.
  /// </summary>
  public class SpreadsheetsManager
  {
    #region Константы

    /// <summary>
    /// Количество столбцов по умолчанию.
    /// </summary>
    public const int DefaultColumnCount = 15;

    /// <summary>
    /// Количество строк по умолчанию.
    /// </summary>
    public const int DefaultRowCount = 15;

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Экземпляр сервиса Google Spreadsheet.
    /// </summary>
    public static SpreadsheetsService Service { get; set; }

    #endregion

    #region Методы

    /// <summary>
    /// Получить все строки старницы.
    /// </summary>
    /// <param name="spreadsheetName">Имя книги (файла).</param>
    /// <param name="worksheetName">Имя страницы.</param>
    /// <param name="defaultColumnHeaders">Имена заголовков колонок.</param>
    /// <returns>Все строки страницы.</returns>
    public static ListFeed GetRows(string spreadsheetName, string worksheetName, string[] defaultColumnHeaders = null)
    {
      ListQuery listQuery = InitializeListBasedQuery(spreadsheetName, worksheetName);
      var listFeed = Service.Query(listQuery);
      if (listFeed.TotalResults == 0 && defaultColumnHeaders != null)
        InsertHeaderRow(spreadsheetName, worksheetName, defaultColumnHeaders);
      return listFeed;
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
      return Service.Query(cellQuery);
    }

    /// <summary>
    /// Получить все строки старницы по заданному фильтру.
    /// </summary>
    /// <param name="spreadsheetName">Имя книги (файла).</param>
    /// <param name="worksheetName">Имя страницы.</param>
    /// <param name="filter">Фильтр.</param>
    /// <param name="defaultColumnHeaders">Имена заголовков колонок.</param>
    /// <returns>Все строки страницы по заданному фильтру.</returns>
    public static ListFeed GetRowsWithFilter(string spreadsheetName, string worksheetName, string filter, string[] defaultColumnHeaders = null)
    {
      ListQuery listQuery = InitializeListBasedQuery(spreadsheetName, worksheetName);
      if (!string.IsNullOrEmpty(filter))
      {
        listQuery.SpreadsheetQuery = filter;
        listQuery.Reverse = true;
      }
      var listFeed = Service.Query(listQuery);
      if (listFeed.TotalResults == 0 && defaultColumnHeaders != null)
        InsertHeaderRow(spreadsheetName, worksheetName, defaultColumnHeaders);
      return listFeed;
    }

    /// <summary>
    /// Инициализировать ListQuery запрос к сервису Google Spreadsheet.
    /// </summary>
    /// <param name="spreadsheetName">Имя книги (файла).</param>
    /// <param name="worksheetName">Имя страницы.</param>
    /// <returns>Запрос к сервису.</returns>
    public static ListQuery InitializeListBasedQuery(string spreadsheetName, string worksheetName)
    {
      var worksheet = GetWorksheet(spreadsheetName, worksheetName);
      var listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
      return new ListQuery(listFeedLink.HRef.ToString());
    }

    /// <summary>
    /// Инициализировать CellQuery запрос к сервису Google Spreadsheet.
    /// </summary>
    /// <param name="spreadsheetName">Имя книги (файла).</param>
    /// <param name="worksheetName">Имя страницы.</param>
    /// <returns>Запрос к сервису.</returns>
    public static CellQuery InitializeCellBasedQuery(string spreadsheetName, string worksheetName)
    {
      var worksheet = GetWorksheet(spreadsheetName, worksheetName);
      return new CellQuery(worksheet.CellFeedLink);
    }

    /// <summary>
    /// Добавить новую строку (не первую) в страницу.
    /// </summary>
    /// <param name="listFeed">Строки страницы.</param>
    /// <param name="rowValues">Значения строки (key - заголовок столбца, value - значение ячейки).</param>
    public static void AddNewRow(ListFeed listFeed, IDictionary<string, string> rowValues)
    {
      var row = new ListEntry();
      foreach (var item in rowValues)
        row.Elements.Add(CreateCell(item));
      Service.Insert(listFeed, row);
    }

    /// <summary>
    /// Получить книгу (файл) по заданному имени.
    /// </summary>
    /// <param name="spreadsheetName">Имя книги (файла).</param>
    /// <returns>Книга (файл).</returns>
    private static SpreadsheetEntry GetSpreadSheet(string spreadsheetName)
    {
      var query = new SpreadsheetQuery();

      return Service.Query(query).Entries
        .OfType<SpreadsheetEntry>()
        .Where(e => e.Title.Text == spreadsheetName)
        .FirstOrDefault();
    }

    /// <summary>
    /// Получить (либо создать новую) страницу из заданной книги с заданным именем страницы.
    /// </summary>
    /// <param name="spreadsheetName">Имя книги (файла).</param>
    /// <param name="worksheetName">Имя страницы.</param>
    /// <returns>Страница.</returns>
    private static WorksheetEntry GetWorksheet(string spreadsheetName, string worksheetName)
    {
      if (Service == null)
        return null;

      var spreadsheet = GetSpreadSheet(spreadsheetName);
      if (spreadsheet == null)
        return null;

      var worksheet = GetWorkSheet(spreadsheet, worksheetName);
      if (worksheet == null)
        worksheet = CreateNewWorkSheet(spreadsheet, worksheetName);
      return worksheet;
    }

    /// <summary>
    /// Получить страницу по заданной книге и имени страницы.
    /// </summary>
    /// <param name="spreadsheet">Книга (файл).</param>
    /// <param name="worksheetName">Имя страницы.</param>
    /// <returns>Страница.</returns>
    private static WorksheetEntry GetWorkSheet(SpreadsheetEntry spreadsheet, string worksheetName)
    {
      return spreadsheet.Worksheets.Entries
        .OfType<WorksheetEntry>()
        .Where(w => w.Title.Text == worksheetName)
        .FirstOrDefault();
    }

    /// <summary>
    /// Создать новую страницу.
    /// </summary>
    /// <param name="spreadsheet">Книга (файл).</param>
    /// <param name="worksheetName">Имя страницы.</param>
    /// <returns></returns>
    private static WorksheetEntry CreateNewWorkSheet(SpreadsheetEntry spreadsheet, string worksheetName)
    {
      var worksheet = new WorksheetEntry
      {
        Cols = DefaultColumnCount,
        Rows = DefaultRowCount
      };
      worksheet.Title.Text = worksheetName;

      WorksheetFeed worksheetFeed = spreadsheet.Worksheets;
      Service.Insert(worksheetFeed, worksheet);

      // переполучаем новую страницу.
      var reloadedSpreadsheet = GetSpreadSheet(spreadsheet.Title.Text);
      return GetWorkSheet(reloadedSpreadsheet, worksheetName);
    }

    /// <summary>
    /// Добавить первую строку на странице (которая в ListBased-нотации считается строкой заголовка).
    /// </summary>
    /// <param name="spreadsheetName">Имя книги (файла).</param>
    /// <param name="worksheetName">Имя страницы.</param>
    /// <param name="defaultColumnHeaders">Имена заголовков колонок.</param>
    private static void InsertHeaderRow(string spreadsheetName, string worksheetName, string[] defaultColumnHeaders)
    {
      // Добавление первой строки не умеет работать в ListBased-нотации, т.к. первая строка считается заголовком и задает некую структуру страницы.
      var cellFeed = GetCells(spreadsheetName, worksheetName);
      for (int i = 1; i <= defaultColumnHeaders.Count(); i++)
      {
        // Отсчет идет от 1, т.к. номера ячеек начинаются от 1.
        var cellEntry = new CellEntry(1, (uint)i, defaultColumnHeaders[i - 1]);
        cellFeed.Insert(cellEntry);
      }
    }

    /// <summary>
    /// Создать ячейку.
    /// </summary>
    /// <param name="cellValue">Значение ячейки (key - заголовок столбца, value - значение ячейки)</param>
    /// <returns>Ячейка.</returns>
    private static ListEntry.Custom CreateCell(KeyValuePair<string, string> cellValue)
    {
      return new ListEntry.Custom() { LocalName = cellValue.Key, Value = cellValue.Value };
    }

    #endregion
  }
}