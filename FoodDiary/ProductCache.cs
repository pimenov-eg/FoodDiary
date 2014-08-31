using FoodDiary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDiary
{
  public static class ProductCache
  {
    public static IEnumerable<Product> AllProducts { get; set; }
  }
}
