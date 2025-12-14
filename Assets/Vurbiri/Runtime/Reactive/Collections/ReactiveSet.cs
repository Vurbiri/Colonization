using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Reactive.Collections
{
    [JsonArray]
    public abstract class ReadOnlyReactiveSet<T> : IReadOnlyCollection<T> where T : class, IReactiveItem<T>
    {
        protected const int BASE_CAPACITY = 5;

        protected T[] _values;
        protected int _capacity;
        protected readonly RInt _count = new(0);
        protected readonly ReactiveVersion<T, TypeEvent> _version = new();

        public T this[int index] { [Impl(256)] get => _values[index]; }

        public int Capacity { [Impl(256)] get => _capacity; }

        public int Count { [Impl(256)] get => _count.Value; }
        public ReactiveValue<int> CountReactive { [Impl(256)] get => _count; }

        public T First
        {
            get
            {
                if (_count != 0)
                {
                    for (int i = 0; i < _capacity; ++i)
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
                    for (int i = 0; i < _capacity; ++i)
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
                for (int i = 0; i < _capacity; ++i)
                {
                    if (_values[i] != null)
                        action(_values[i], TypeEvent.Subscribe);
                }
            }

            return _version.Add(action);
        }

        public bool Contains(T item)
        {
            int index = item != null ? item.Index : -1;
            return index >= 0 & index < _capacity && _values[index].Equals(item);
        }

        [Impl(256)] public SetEnumerator<T> GetEnumerator() => new(_values, _capacity, _version);
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => _count == 0 ? EmptyEnumerator<T>.Instance : GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>)this).GetEnumerator();
    }

    //**********************************************************************************************

    [JsonArray]
    public class ReactiveSet<T> : ReadOnlyReactiveSet<T>, IDisposable where T : class, IReactiveItem<T>
    {
        public ReactiveSet()
        {
            _values = new T[_capacity = BASE_CAPACITY];
        }

        public ReactiveSet(int capacity)
        {
            _capacity = capacity;
            _values = capacity == 0 ? Array.Empty<T>() : new T[capacity];
        }

        public void Add(T item)
        {
            int count = _count;

            if (count == _capacity)
            {
                _capacity = _capacity << 1 | BASE_CAPACITY;
                Array.Resize(ref _values, _capacity);

                AddItem(item, count);
            }
            else
            {
                for (int i = 0; i < _capacity; ++i)
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
            for (int i = 0; i < _capacity; ++i)
                _values[i]?.Dispose();

            _values = null;
        }

        [Impl(256)]
        private void AddItem(T item, int index)
        {
            _values[index] = item;
            item.Adding(RedirectEvents, index);
            _count.Increment();
        }

        private void RedirectEvents(T item, TypeEvent operation)
        {
            if (operation == TypeEvent.Change || operation == TypeEvent.Subscribe)
            {
                _version.Signal(item, operation);
                return;
            }

            if (operation == TypeEvent.Remove)
            {
                _values[item.Index] = null;
                _count.Decrement();
            }

            _version.Next(item, operation);
        }
    }
}
