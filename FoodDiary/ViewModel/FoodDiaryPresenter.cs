using FoodDiary.Interfaces;
using FoodDiary.Model;
using Google;
using Google.GData.Client;
using SimpleMVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Google.GData.Spreadsheets;

namespace FoodDiary.ViewModel
{
  public class FoodDiaryPresenter : ObservableObject
  {
    private readonly IProductStorage productStorage = new GoogleProductStorage();

    private readonly IPortionStorage portionStorage = new GooglePortionStorage();

    private IEnumerable<Product> allProducts;

    public IEnumerable<Product> AllProducts
    {
      get
      {
        return this.allProducts;
      }

      set
      {
        this.allProducts = value;
        RaisePropertyChangedEvent("AllProducts");
      }
    }

    public Product selectedProduct;

    public Product SelectedProduct
    {
      get
      {
        return this.selectedProduct;
      }

      set
      {
        this.selectedProduct = value;
        RaisePropertyChangedEvent("SelectedProduct");
      }
    }

    public string selectedProductWeight;

    public string SelectedProductWeight
    {
      get
      {
        return this.selectedProductWeight;
      }

      set
      {
        this.selectedProductWeight = value;
        RaisePropertyChangedEvent("SelectedProductWeight");
      }
    }

    private OneTimePortion selectedOneTimePortion;

    public OneTimePortion SelectedOneTimePortion
    {
      get
      {
        return this.selectedOneTimePortion;
      }

      set
      {
        this.selectedOneTimePortion = value;
        RaisePropertyChangedEvent("SelectedOneTimePortion");
      }
    }

    public ObservableRangeCollection<OneTimePortion> DailyPortion { get; set; }

    public string CalResult
    {
      get
      {
        // TODO сделать нормально
        var dailyPortion = new DailyPortion
        {
          Date = DateTime.Now.ToShortDateString(),
          AllEatingProducts = new List<OneTimePortion>(this.DailyPortion)
        };

        // текущий день заполняется без данных о калорийности... надо продернуть
        return dailyPortion.TotalCalValue.ToString("N");
      }
    }

    private void SpreadsheetsServiceInitializedHandler(object sender, EventArgs e)
    {
      this.AllProducts = this.productStorage.GetAllProducts();
      // TODO сделать нормальное означивание с подпиской
      var currentDaily = this.portionStorage.GetCurrentDailyPortion().AllEatingProducts;
      this.DailyPortion.AddRange(currentDaily);
      //this.RaisePropertyChangedEvent("DailyPortion");
    }

    #region Команды добавления/удаления продуктов из дневного рациона

    public ICommand AddToDailyPortionCommand { get { return new DelegateCommand(this.AddToDailyPortion); } }

    private void AddToDailyPortion(object parameter)
    {
      var oneTimePortion = new OneTimePortion
      {
        Product = this.SelectedProduct,
        Weight = Convert.ToInt32(this.SelectedProductWeight)
      };
      this.DailyPortion.Add(oneTimePortion);
    }

    public ICommand RemoveFromDailyPortionCommand { get { return new DelegateCommand(this.RemoveFromDailyPortion); } }

    private void RemoveFromDailyPortion(object parameter)
    {
      this.DailyPortion.Remove(this.SelectedOneTimePortion);
    }

    #endregion

    #region Команда добавления нового продукта

    public ICommand AddProductCommand { get { return new DelegateCommand(this.AddProduct); } }

    private void AddProduct(object parameter)
    {
      new AddProductWindow().ShowDialog();
    }

    #endregion

    #region Конструкторы

    public FoodDiaryPresenter()
    {
      GoogleAuthorizationManager.SpreadsheetsServiceInitialized += this.SpreadsheetsServiceInitializedHandler;
      this.DailyPortion = new ObservableRangeCollection<OneTimePortion>();
      this.DailyPortion.CollectionChanged += DailyPortionCollectionChangedHandler;
    }

    void DailyPortionCollectionChangedHandler(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      RaisePropertyChangedEvent("CalResult");
    }

    public ICommand TestCommand { get { return new DelegateCommand(this.Test); } }

    private void Test(object parameter)
    {
      
    }

    #endregion

    #region Команда сохранения дневного рациона в Google-таблицу

    public ICommand SaveDailyPortionCommand { get { return new DelegateCommand(this.SaveDailyPortion); } }

    private void SaveDailyPortion(object parameter)
    {
      var dailyPortion = new DailyPortion
      {
        Date = DateTime.Now.ToShortDateString(),
        AllEatingProducts = new List<OneTimePortion>(this.DailyPortion)
      };
      this.portionStorage.SaveDailyPortion(dailyPortion);
    }

    #endregion
  }
}
