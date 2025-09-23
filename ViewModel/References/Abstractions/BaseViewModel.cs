using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DocumentHelper.ViewModel.References.Abstractions
{
    public abstract class BaseViewModel : ICloneable, INotifyPropertyChanged 
    {
        public object Clone()
        {
            return MemberwiseClone();
        }

        // Реализация изменения поля
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void SetValueField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            field = value;
            OnPropertyChanged(propertyName);
        }

        public void SetValueProperty<T>(T value, Action<T> setter, [CallerMemberName] string propertyName = null)
        {
            setter(value);
            OnPropertyChanged(propertyName);
        }

    }
}
