using Kasay.PropertyChanged.Infrastructure;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kasay.PropertyChanged
{
    public abstract class Notifiable : INotifyPropertyChanged, IDisposable
    {
        internal static UnsubscriptionCollector UnsubscriptionCollector { get; } = UnsubscriptionCollector ?? new UnsubscriptionCollector();

        public event PropertyChangedEventHandler PropertyChanged;

        protected Boolean SetProperty<T>(
            ref T storage, 
            T value, 
            [CallerMemberName] String propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }
            else
            {
                storage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

                return true;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose()
        {
            UnsubscriptionCollector.UnsubscribeAndRemove(this);
        }
    }
}
