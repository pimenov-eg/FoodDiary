using FoodDiary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDiary.Interfaces
{
  public interface IPortionStorage
  {
    void SaveDailyPortion(DailyPortion dailyPortion);

    DailyPortion GetCurrentDailyPortion();
  }
}
