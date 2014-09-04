using FoodDiary.Model;

namespace FoodDiary.Interfaces
{
  /// <summary>
  /// Интерфейс хранилища дневного рациона.
  /// </summary>
  public interface IPortionStorage
  {
    /// <summary>
    /// Сохранить дневной рацион.
    /// </summary>
    /// <param name="dailyPortion">Дневной рацион.</param>
    void SaveDailyPortion(DailyPortion dailyPortion);

    /// <summary>
    /// Получить текущий дневной рацион.
    /// </summary>
    /// <returns></returns>
    DailyPortion GetCurrentDailyPortion();
  }
}