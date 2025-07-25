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
            int index = item != null ? item.Index : -1;
            return index >= 0 & index < _capacity && _values[index].Equals(item);
        }

        public T GetRandom()
        {
            if(_count != 0)
            {
                int index = UnityEngine.Random.Range(0, _count);
                for (int i = 0; i < _capacity; i++)
                    if (_values[i] != null && index-- == 0)
                        return _values[i];
            }
            return null;
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
