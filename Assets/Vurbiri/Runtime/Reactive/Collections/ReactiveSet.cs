using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    [JsonArray]
    public abstract class ReadOnlyReactiveSet<T> : IReadOnlyCollection<T> where T : class, IReactiveItem<T>
    {
        protected const int BASE_CAPACITY = 7;

        protected T[] _values;
        protected int _capacity = BASE_CAPACITY;
        protected readonly RInt _count = new(0);

        protected readonly VAction<T, TypeEvent> _changeEvent = new();

        public int Capacity => _capacity;
        public int Count => _count.Value;
        public ReactiveValue<int> CountReactive => _count;

        public T First
        {
            get
            {
                if (_count != 0)
                {
                    for (int i = 0; i < _capacity; i++)
                        if (_values[i] != null)
                            return _values[i];
                }
                return null;
            }
        }

        public T Random
        {
            get
            {
                if (_count != 0)
                {
                    int index = UnityEngine.Random.Range(0, _count);
                    for (int i = 0; i < _capacity; i++)
                        if (_values[i] != null && index-- == 0)
                            return _values[i];
                }
                return null;
            }
        }

        public Subscription Subscribe(Action<T, TypeEvent> action, bool instantGetValue = true)
        {
            if (instantGetValue)
            {
                for (int i = 0; i < _capacity; i++)
                {
                    if (_values[i] != null)
                        action(_values[i], TypeEvent.Subscribe);
                }
            }

            return _changeEvent.Add(action);
        }

        public bool Contains(T item)
        {
            int index = item != null ? item.Index : -1;
            return index >= 0 & index < _capacity && _values[index].Equals(item);
        }

        public IEnumerator<T> GetEnumerator() => new SetEnumerator<T>(_values);
        IEnumerator IEnumerable.GetEnumerator() => new SetEnumerator<T>(_values);
    }

    //**********************************************************************************************

    [JsonArray]
    public class ReactiveSet<T> : ReadOnlyReactiveSet<T>, IDisposable where T : class, IReactiveItem<T>
    {
        public ReactiveSet()
        {
            _values = new T[_capacity];
        }

        public ReactiveSet(int capacity)
        {
            _capacity = capacity;
            _values = new T[_capacity];
        }

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
            }
            else
            {
                for (int i = 0; i < _capacity; i++)
                {
                    if (_values[i] == null)
                    {
                        AddItem(item, i);
                        return;
                    }
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

        public void Dispose()
        {
            for (int i = 0; i < _capacity; i++)
                _values[i]?.Dispose();

            _values = null;
        }

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

            _changeEvent.Invoke(item, operation);
        }
    }
}
