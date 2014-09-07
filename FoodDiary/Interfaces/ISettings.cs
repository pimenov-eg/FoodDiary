using System.Collections.Generic;
using FoodDiary.Model;

namespace FoodDiary.Interfaces
{
  /// <summary>
  /// Интерфейс базовых настроек дневника питания.
  /// </summary>
  public interface ISettings
  {
    /// <summary>
    /// Моя текущая масса.
    /// </summary>
    float Weight { get; }

    /// <summary>
    /// Признак того, что цель - это набор массы.
    /// </summary>
    bool IsMassGoal { get; }

    /// <summary>
    /// Требуемое количество ккал на килограмм веса.
    /// </summary>
    float NeedCcal { get; }

    /// <summary>
    /// Требуемое количество белка на килограмм веса.
    /// </summary>
    float NeedProtein { get; }

    /// <summary>
    /// Требуемое количество углеводов на килограмм веса.
    /// </summary>
    float NeedCarbohydrate { get; }

    /// <summary>
    /// Требуемое количество жира на килограмм веса.
    /// </summary>
    float NeedFat { get; }

    /// <summary>
    /// Инициализировать настройки.
    /// </summary>
    void Initialize();
  }
}