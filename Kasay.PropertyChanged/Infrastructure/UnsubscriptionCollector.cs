using System;
using System.Collections.Generic;
using System.Linq;

namespace Kasay.PropertyChanged.Infrastructure
{
    public class UnsubscriptionCollector : List<KeyValuePair<Object, Action>>
    {
        public void Add(Object key, Action value)
        {
            var element = new KeyValuePair<Object, Action>(key, value);

            Add(element);
        }

        public void UnsubscribeAndRemove(Object obj)
        {
            var filter = this
               .Where(_ => _.Key == obj)
               .ToList();

            foreach (var item in filter)
            {
                //Unsubscribe
                item.Value.Invoke();
                Remove(item);
            }
        }
    }
}
