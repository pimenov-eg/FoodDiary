namespace FoodDiary.Model
{
  /// <summary>
  /// Продукт.
  /// </summary>
  public class Product
  {
    #region Поля и свойства

    /// <summary>
    /// Наименование продукта.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Количество килокалорий на 100гр.
    /// </summary>
    public float CalValue { get; set; }

    /// <summary>
    /// Количество белка на 100гр.
    /// </summary>
    public float Protein { get; set; }

    /// <summary>
    /// Количество углеводов на 100гр.
    /// </summary>
    public float Carbohydrate { get; set; }

    /// <summary>
    /// Количество жира на 100гр.
    /// </summary>
    public float Fat { get; set; }

    #endregion

    #region Методы

    public override string ToString()
    {
      return string.Format("{0}, {1} ккал", this.Name, this.CalValue);
    }

    #endregion
  }
}