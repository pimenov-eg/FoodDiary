using FoodDiary.Interfaces;
using FoodDiary.Model;
using Google;
using Google.GData.Spreadsheets;
using System;
using System.Globalization;

namespace FoodDiary
{
  /// <summary>
  /// Настройки дневника питания в сервисе Google Spreadsheet.
  /// </summary>
  public class GoogleSettings : ISettings
  {
    #region Поля и свойства

    /// <summary>
    /// Имя файла.
    /// </summary>
    private static readonly string SpreadSheetName = FoodDiary.Properties.Settings.Default.SpreadSheetName;

    /// <summary>
    /// Имя листа с настройками.
    /// </summary>
    private static readonly string SettingsWorkSheetName = FoodDiary.Properties.Settings.Default.SettingsWorkSheetName;

    public float Weight { get; private set; }

    public bool IsMassGoal { get; private set; }

    public float NeedCcal { get; private set; }

    public float NeedProtein { get; private set; }

    public float NeedCarbohydrate { get; private set; }

    public float NeedFat { get; private set; }

    #endregion

    #region Методы

    public void Initialize()
    {
      var listFeed = SpreadsheetsManager.GetRows(SpreadSheetName, SettingsWorkSheetName);
      if (listFeed == null)
        throw new Exception("Settings not found");

      foreach (ListEntry row in listFeed.Entries)
      {
        var product = new Product();
        foreach (ListEntry.Custom element in row.Elements)
        {
          switch (element.LocalName)
          {
            case "weight":
              this.Weight = float.Parse(element.Value, CultureInfo.InvariantCulture.NumberFormat);
              break;
            case "ismassgoal":
              this.IsMassGoal = bool.Parse(element.Value);
              break;
            case "needccal":
              this.NeedCcal = float.Parse(element.Value, CultureInfo.InvariantCulture.NumberFormat);
              break;
            case "needprotein":
              this.NeedProtein = float.Parse(element.Value, CultureInfo.InvariantCulture.NumberFormat);
              break;
            case "needcarbohydrate":
              this.NeedCarbohydrate = float.Parse(element.Value, CultureInfo.InvariantCulture.NumberFormat);
              break;
              case "needfat":
              this.NeedFat = float.Parse(element.Value, CultureInfo.InvariantCulture.NumberFormat);
              break;
            default:
              break;
          }
        }
      }
    }

    #endregion

  }
}