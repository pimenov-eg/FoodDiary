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
using System.Globalization;

namespace FoodDiary.ViewModel
{
  public class AddProductPresenter : ObservableObject
  {
    private readonly IProductStorage productStorage = new GoogleProductStorage();

    private string name;

    public string Name
    {
      get
      {
        return this.name;
      }

      set
      {
        this.name = value;
        RaisePropertyChangedEvent("Name");
      }
    }

    private float calValue;

    public float CalValue
    {
      get
      {
        return this.calValue;
      }

      set
      {
        this.calValue = value;
        RaisePropertyChangedEvent("CalValue");
      }
    }

    private float protein;

    public float Protein
    {
      get
      {
        return this.protein;
      }

      set
      {
        this.protein = value;
        RaisePropertyChangedEvent("Protein");
      }
    }

    private float carbohydrate;

    public float Carbohydrate
    {
      get
      {
        return this.carbohydrate;
      }

      set
      {
        this.carbohydrate = value;
        RaisePropertyChangedEvent("Carbohydrate");
      }
    }

    private float fat;

    public float Fat
    {
      get
      {
        return this.fat;
      }

      set
      {
        this.fat = value;
        RaisePropertyChangedEvent("Fat");
      }
    }

    #region Команда добавления нового продукта

    public ICommand AddProductCommand { get { return new DelegateCommand(this.AddProduct); } }

    private void AddProduct()
    {
      // посмотреть преобразование типов (в каком типе писать в google - в строке или числом)
      var product = new Product
      {
        Name = this.Name,
        CalValue = this.CalValue,
        Protein = this.Protein,
        Carbohydrate = this.Carbohydrate,
        Fat = this.Fat
      };
      this.productStorage.AddProduct(product);
    }

    #endregion

    #region Конструкторы

    public AddProductPresenter()
    {
    }

    #endregion
  }
}