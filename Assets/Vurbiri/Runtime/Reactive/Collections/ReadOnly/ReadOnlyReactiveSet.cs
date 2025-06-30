using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    [JsonArray]
    public abstract class ReadOnlyReactiveSet<T> : IReactiveSet<T> where T : class, IReactiveItem<T>
    {
        protected const int BASE_CAPACITY = 7;

        protected T[] _values;
        protected int _capacity = BASE_CAPACITY;
        protected readonly RInt _count = new(0);

        protected readonly Subscription<T, TypeEvent> _subscriber = new();

        public int Capacity => _capacity;
        public int Count => _count.Value;
        public IReactiveValue<int> CountReactive => _count;

        #region IReactiveCollection
        public Unsubscription Subscribe(Action<T, TypeEvent> action, bool instantGetValue = true)
        {
            if (instantGetValue)
            {
                for (int i = 0; i < _capacity; i++)
                {
                    if (_values[i] != null)
                        action(_values[i], TypeEvent.Subscribe);
                }
            }

            return _subscriber.Add(action);
        }
        #endregion

        public bool Contains(T item)
        {
            if (item == null) return false;

            int index = item.Index;
            return index >= 0 & index < _capacity && _values[index].Equals(item);
        }

        public void Dispose()
        {
            for (int i = 0; i < _capacity; i++)
                _values[i]?.Dispose();

            _values = null;
        }

        public IEnumerator<T> GetEnumerator() => new SetEnumerator<T>(_values);
        IEnumerator IEnumerable.GetEnumerator() => new SetEnumerator<T>(_values);
    }
}
