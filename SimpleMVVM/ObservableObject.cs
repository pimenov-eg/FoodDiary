using System.ComponentModel;

namespace SimpleMVVM
{
  /// <summary>
  /// Базовый объект, реализующий интерфейс INotifyPropertyChanged.
  /// </summary>
  public class ObservableObject : INotifyPropertyChanged
  {
    /// <summary>
    /// Событие на изменение свойства.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Сгенерировать событие на изменение свойства.
    /// </summary>
    /// <param name="propertyName">Имя свойства.</param>
    protected void RaisePropertyChangedEvent(string propertyName)
    {
      var handler = PropertyChanged;
      if (handler != null)
        handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}