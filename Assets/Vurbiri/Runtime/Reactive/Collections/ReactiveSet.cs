using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    [JsonArray]
    public class ReactiveSet<T> : IReactiveSet<T> where T : class, IReactiveItem<T>
    {
        private const int BASE_CAPACITY = 7;

        protected T[] _values;
        protected int _capacity = BASE_CAPACITY;
        protected readonly RInt _count = new(0);

        protected readonly Subscription<T, TypeEvent> _subscriber = new();

        public int Capacity => _capacity;
        public int Count => _count.Value;
        public IReactiveValue<int> CountReactive => _count;

        #region Constructors
        public ReactiveSet()
        {
            _values = new T[_capacity];
        }

        public ReactiveSet(int capacity)
        {
            _capacity = capacity;
            _values = new T[_capacity];
        }
        #endregion

        #region IReactiveCollection
        public Unsubscription Subscribe(Action<T, TypeEvent> action, bool instantGetValue = true)
        {
            if (instantGetValue)
            {
                for (int i = 0; i < _capacity; i++)
                {
                    if(_values[i] != null)
                        action(_values[i], TypeEvent.Subscribe);
                }
            }

            return _subscriber.Add(action);
        }
        #endregion

        public void Add(T item)
        {
            if (_count == _capacity)
            {
                _capacity = _capacity << 1 | BASE_CAPACITY;

                T[] array = new T[_capacity];
                for (int i = 0; i < _count; i++)
                    array[i] = _values[i];
                _values = array;

                AddItem(item, _count);
                return;
            }

            for (int i = 0; i < _capacity; i++)
            {
                if (_values[i] == null)
                {
                    AddItem(item, i);
                    return;
                }
            }
        }

        public bool Remove(T item)
        {
            if (!Contains(item))
                return false;

            _values[item.Index].Removing();
            return true;
        }

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

        private void AddItem(T item, int index)
        {
            _values[index] = item;
            item.Adding(RedirectEvents, index);
            _count.Increment();
        }

        private void RedirectEvents(T item, TypeEvent operation)
        {
            if (operation == TypeEvent.Remove)
            {
                _values[item.Index] = null;
                _count.Decrement();
            }

            _subscriber.Invoke(item, operation);
        }
    }
}
