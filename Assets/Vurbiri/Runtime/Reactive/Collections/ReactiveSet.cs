//Assets\Vurbiri\Runtime\Reactive\Collections\ListReactiveItems.cs
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    [JsonArray]
    public class ReactiveSet<T> : IReactiveSet<T> where T : class, IReactiveItem<T>
    {
        protected T[] _values;
        protected int _capacity = 4;
        protected readonly RInt _count = new(0);

        protected readonly Subscriber<T, TypeEvent> _subscriber = new();

        public int Capacity => _capacity;
        public int Count => _count;
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
        public Unsubscriber Subscribe(Action<T, TypeEvent> action, bool sendCallback = true)
        {
            if (sendCallback)
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
                GrowArray();
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
            return index >= 0 & index < _capacity && EqualityComparer<T>.Default.Equals(_values[index], item);
        }

        public IEnumerator<T> GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

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

            _subscriber.Invoke(item, operation);
        }

        private void GrowArray()
        {
            _capacity = _capacity << 1 | 4;

            T[] array = new T[_capacity];
            for (int i = 0; i < _count; i++)
                array[i] = _values[i];
            _values = array;
        }

        #region Nested classes: IdSetEnumerator
        //***********************************
        public class Enumerator : IEnumerator<T>
        {
            private readonly T[] _values;
            private readonly int _capacity;
            private int _cursor = -1;
            private T _current;

            public T Current => _current;
            object IEnumerator.Current => _current;

            public Enumerator(ReactiveSet<T> parent)
            {
                _values = parent._values;
                _capacity = parent._capacity;
            }

            public bool MoveNext()
            {
                if (++_cursor >= _capacity)
                    return false;

                _current = _values[_cursor];

                if (_current == null)
                    return MoveNext();

                return true;
            }

            public void Reset() => _cursor = -1;

            public void Dispose() { }
        }
        #endregion
    }
}
