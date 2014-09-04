using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using FoodDiary.Interfaces;
using FoodDiary.Model;
using Google;
using SimpleMVVM;

namespace FoodDiary.ViewModel
{
  /// <summary>
  /// Презентер главного окна.
  /// </summary>
  public class FoodDiaryPresenter : ObservableObject
  {
    #region Поля и свойства

    /// <summary>
    /// Хранилище продуктов.
    /// </summary>
    private readonly IProductStorage productStorage = new GoogleProductStorage();

    /// <summary>
    /// Хранилище текущего рациона.
    /// </summary>
    private readonly IPortionStorage portionStorage = new GooglePortionStorage();

    /// <summary>
    /// Признак того, что сервис Google Spreadsheet инициализирован.
    /// </summary>
    public bool IsSpreadsheetsServiceInitialized { get; private set; }

    private IEnumerable<Product> allProducts;

    /// <summary>
    /// Все продукты.
    /// </summary>
    public IEnumerable<Product> AllProducts
    {
      get
      {
        return this.allProducts;
      }

      set
      {
        this.allProducts = value;
        this.RaisePropertyChangedEvent("AllProducts");
      }
    }

    public Product selectedProduct;

    /// <summary>
    /// Выбранный продукт.
    /// </summary>
    public Product SelectedProduct
    {
      get
      {
        return this.selectedProduct;
      }

      set
      {
        this.selectedProduct = value;
        this.RaisePropertyChangedEvent("SelectedProduct");
      }
    }

    public string selectedProductWeight;

    /// <summary>
    /// Масса выбранного продукта в граммах.
    /// </summary>
    public string SelectedProductWeight
    {
      get
      {
        return this.selectedProductWeight;
      }

      set
      {
        this.selectedProductWeight = value;
        this.RaisePropertyChangedEvent("SelectedProductWeight");
      }
    }

    private OneTimePortion selectedOneTimePortion;

    /// <summary>
    /// Разовая порция выбранного продукта.
    /// </summary>
    public OneTimePortion SelectedOneTimePortion
    {
      get
      {
        return this.selectedOneTimePortion;
      }

      set
      {
        this.selectedOneTimePortion = value;
        this.RaisePropertyChangedEvent("SelectedOneTimePortion");
      }
    }

    /// <summary>
    /// Дневной рацион.
    /// </summary>
    public ObservableRangeCollection<OneTimePortion> DailyPortion { get; set; }

    /// <summary>
    /// Итоговая дневная калорийность.
    /// </summary>
    public string CalResult
    {
      get
      {
        var dailyPortion = new DailyPortion
        {
          Date = DateTime.Now.ToShortDateString(),
          AllEatingProducts = new List<OneTimePortion>(this.DailyPortion)
        };

        return dailyPortion.TotalCalValue.ToString("N");
      }
    }

    #endregion

    #region Методы

    /// <summary>
    /// Обработчик события на завершение инициализации сервиса Google Spreadsheet.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void SpreadsheetsServiceInitializedHandler(object sender, EventArgs e)
    {
      this.IsSpreadsheetsServiceInitialized = true;
      this.RaisePropertyChangedEvent("IsSpreadsheetsServiceInitialized");

      this.AllProducts = this.productStorage.GetAllProducts();
      var currentDaily = this.portionStorage.GetCurrentDailyPortion().AllEatingProducts;
      this.DailyPortion.AddRange(currentDaily);
    }

    /// <summary>
    /// Обработчик события на изменение коллекции дневного рациона.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Обработчик события.</param>
    private void DailyPortionCollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.RaisePropertyChangedEvent("CalResult");
    }

    #endregion

    #region Команды добавления/удаления продуктов из дневного рациона

    public ICommand AddToDailyPortionCommand { get { return new DelegateCommand(this.AddToDailyPortion, this.CanAddToDailyPortion); } }

    private void AddToDailyPortion(object parameter)
    {
      var oneTimePortion = new OneTimePortion
      {
        Product = this.SelectedProduct,
        Weight = float.Parse(this.SelectedProductWeight, CultureInfo.InvariantCulture.NumberFormat)
      };
      this.DailyPortion.Add(oneTimePortion);
    }

    private bool CanAddToDailyPortion(object parameter)
    {
      return this.IsSpreadsheetsServiceInitialized &&
        this.SelectedProduct != null && !string.IsNullOrWhiteSpace(this.SelectedProductWeight);
    }

    public ICommand RemoveFromDailyPortionCommand { get { return new DelegateCommand(this.RemoveFromDailyPortion, this.CanRemoveFromDailyPortion); } }

    private void RemoveFromDailyPortion(object parameter)
    {
      this.DailyPortion.Remove(this.SelectedOneTimePortion);
    }

    private bool CanRemoveFromDailyPortion(object parameter)
    {
      return this.IsSpreadsheetsServiceInitialized &&
        this.DailyPortion.Any();
    }

    #endregion

    #region Команда добавления нового продукта

    public ICommand AddProductCommand { get { return new DelegateCommand(this.AddProduct, this.CanAddProduct); } }

    private void AddProduct(object parameter)
    {
      new AddProductWindow().ShowDialog();
    }

    private bool CanAddProduct(object parameter)
    {
      return this.IsSpreadsheetsServiceInitialized;
    }

    #endregion

    #region Команда сохранения дневного рациона в Google-таблицу

    public ICommand SaveDailyPortionCommand { get { return new DelegateCommand(this.SaveDailyPortion, this.CanSaveDailyPortion); } }

    private void SaveDailyPortion(object parameter)
    {
      var dailyPortion = new DailyPortion
      {
        Date = DateTime.Now.ToShortDateString(),
        AllEatingProducts = new List<OneTimePortion>(this.DailyPortion)
      };
      this.portionStorage.SaveDailyPortion(dailyPortion);
    }

    private bool CanSaveDailyPortion(object parameter)
    {
      return this.IsSpreadsheetsServiceInitialized &&
        this.DailyPortion.Any();
    }

    #endregion

    #region Конструкторы

    public FoodDiaryPresenter()
    {
      GoogleAuthorizationManager.SpreadsheetsServiceInitialized += this.SpreadsheetsServiceInitializedHandler;
      this.DailyPortion = new ObservableRangeCollection<OneTimePortion>();
      this.DailyPortion.CollectionChanged += this.DailyPortionCollectionChangedHandler;
    }

    #endregion
  }
}