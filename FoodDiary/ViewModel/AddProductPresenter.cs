using System.Globalization;
using System.Windows.Input;
using FoodDiary.Interfaces;
using FoodDiary.Model;
using SimpleMVVM;
using System.Windows;

namespace FoodDiary.ViewModel
{
  /// <summary>
  /// Презентер окна, добавления нового продукта.
  /// </summary>
  public class AddProductPresenter : ObservableObject
  {
    #region Поля и свойства

    /// <summary>
    /// Хранилище продуктов.
    /// </summary>
    private readonly IProductStorage productStorage = new GoogleProductStorage();

    private string name;

    /// <summary>
    /// Название продукта.
    /// </summary>
    public string Name
    {
      get
      {
        return this.name;
      }

      set
      {
        this.name = value;
        this.RaisePropertyChangedEvent("Name");
      }
    }

    private string calValue;

    /// <summary>
    /// Значение калорийности продукта.
    /// </summary>
    public string CalValue
    {
      get
      {
        return this.calValue;
      }

      set
      {
        this.calValue = value;
        this.RaisePropertyChangedEvent("CalValue");
      }
    }

    private string protein;

    /// <summary>
    /// Количество белков в граммах.
    /// </summary>
    public string Protein
    {
      get
      {
        return this.protein;
      }

      set
      {
        this.protein = value;
        this.RaisePropertyChangedEvent("Protein");
      }
    }

    private string carbohydrate;

    /// <summary>
    /// Количество углеводов в граммах.
    /// </summary>
    public string Carbohydrate
    {
      get
      {
        return this.carbohydrate;
      }

      set
      {
        this.carbohydrate = value;
        this.RaisePropertyChangedEvent("Carbohydrate");
      }
    }

    private string fat;

    /// <summary>
    /// Количество жира в граммах.
    /// </summary>
    public string Fat
    {
      get
      {
        return this.fat;
      }

      set
      {
        this.fat = value;
        this.RaisePropertyChangedEvent("Fat");
      }
    }

    #endregion

    #region Команда добавления нового продукта

    public ICommand AddProductCommand { get { return new DelegateCommand(this.AddProduct, this.CanAddProduct); } }

    private void AddProduct(object parameter)
    {
      var product = new Product
      {
        Name = this.Name,
        CalValue = this.CalValue != null ? float.Parse(this.CalValue, CultureInfo.InvariantCulture.NumberFormat) : 0,
        Protein = this.Protein != null ? float.Parse(this.Protein, CultureInfo.InvariantCulture.NumberFormat) : 0,
        Carbohydrate = this.Carbohydrate != null ? float.Parse(this.Carbohydrate, CultureInfo.InvariantCulture.NumberFormat) :0,
        Fat = this.Fat != null ? float.Parse(this.Fat, CultureInfo.InvariantCulture.NumberFormat) : 0
      };
      if (ProductCache.ContainsProduct(product))
        MessageBox.Show(string.Format("Продукт '{0}' уже есть в хранилище", product.Name));
      else
        this.productStorage.AddProduct(product);
    }

    private bool CanAddProduct(object parameter)
    {
      return !string.IsNullOrWhiteSpace(this.Name) &&
        (!string.IsNullOrWhiteSpace(this.CalValue) && float.Parse(this.CalValue, CultureInfo.InvariantCulture.NumberFormat) != 0) &&
        (
          (!string.IsNullOrWhiteSpace(this.Protein) && float.Parse(this.Protein, CultureInfo.InvariantCulture.NumberFormat) != 0) ||
          (!string.IsNullOrWhiteSpace(this.Carbohydrate) && float.Parse(this.Carbohydrate, CultureInfo.InvariantCulture.NumberFormat) != 0) ||
          (!string.IsNullOrWhiteSpace(this.Fat) && float.Parse(this.Fat, CultureInfo.InvariantCulture.NumberFormat) != 0)
        );
    }

    #endregion

    #region Конструкторы

    public AddProductPresenter()
    {
    }

    #endregion
  }
}