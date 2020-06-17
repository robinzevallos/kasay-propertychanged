using System;
using System.ComponentModel;

namespace Kasay.PropertyChanged.Infrastructure
{
    internal class OnChangedSubscriber
    {
        readonly PropertyChangedEventHandler handler;
        readonly INotifyPropertyChanged notify;

        public OnChangedSubscriber(
            INotifyPropertyChanged notify,
            Action callback,
            String propertyName)
        {
            this.notify = notify;

            handler = (s, e) =>
            {
                if (propertyName == e.PropertyName)
                {
                    callback?.Invoke();
                }
            };

            Subscribe();

            Notifiable.UnsubscriptionCollector.Add(notify, Unsubscribe);
        }

        void Subscribe()
        {
            notify.PropertyChanged += handler;
        }

        void Unsubscribe()
        {
            notify.PropertyChanged -= handler;
        }
    }
}
