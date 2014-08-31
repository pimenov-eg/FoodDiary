﻿using FoodDiary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDiary.Interfaces
{
  /// <summary>
  /// Интерфейс хранилища продуктов.
  /// </summary>
  public interface IProductStorage
  {
    /// <summary>
    /// Получить все продукты.
    /// </summary>
    /// <returns>Список всех продуктов.</returns>
    IEnumerable<Product> GetAllProducts();

    /// <summary>
    /// Добавить новый продукт.
    /// </summary>
    /// <param name="product">Продукт.</param>
    void AddProduct(Product product);
  }
}
