using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Reactive.Collections
{
    [JsonArray]
    public abstract class ReadOnlyReactiveList<T> : IReadOnlyList<T>
    {
        protected static readonly IEqualityComparer<T> s_comparer = EqualityComparer<T>.Default;

        protected T[] _values;
        protected readonly RInt _count = new(0);

        protected readonly VAction<int, T, TypeEvent> _changeEvent = new();

        public T this[int index] 
        {
            [Impl(256)] get
            {
                Throw.IfIndexOutOfRange(index, _count);
                return _values[index];
            }
        }

        public int Count { [Impl(256)] get => _count.Value; }
        public ReactiveValue<int> CountReactive { [Impl(256)] get => _count; }

        public Subscription Subscribe(Action<int, T, TypeEvent> action, bool instantGetValue = true)
        {
            if (instantGetValue)
            {
                for (int i = 0; i < _count; ++i)
                    action(i, _values[i], TypeEvent.Subscribe);
            }

            return _changeEvent.Add(action);
        }
        public void Unsubscribe(Action<int, T, TypeEvent> action) => _changeEvent.Remove(action);

        [Impl(256)] public bool Contains(T item) => IndexOf(item) >= 0;

        public int IndexOf(T item)
        {
            int i = _count.Value;
            while (i --> 0 && !s_comparer.Equals(_values[i], item));
            return i;
        }

        public IEnumerator<T> GetEnumerator() => new ArrayEnumerator<T>(_values, _count.Value);
        IEnumerator IEnumerable.GetEnumerator() => new ArrayEnumerator<T>(_values, _count.Value);
    }

    //**********************************************************************************************
    
    [JsonArray]
    public class ReactiveList<T> : ReadOnlyReactiveList<T>, IList<T>
    {
        private const int BASE_CAPACITY = 7;
        private int _capacity;

        public bool IsReadOnly => false;

        public new T this[int index] 
        {
            [Impl(256)] get
            {
                Throw.IfIndexOutOfRange(index, _count);
                return _values[index];
            }
            [Impl(256)] set
            {
                Throw.IfIndexOutOfRange(index, _count);
                if (!s_comparer.Equals(_values[index], value))
                {
                    _values[index] = value;
                    _changeEvent.Invoke(index, value, TypeEvent.Change);
                }
            }
        }

        #region Constructors
        [Impl(256)] public ReactiveList() : this(BASE_CAPACITY) {}
        [Impl(256)] public ReactiveList(int capacity)
        {
            _capacity = capacity;
            _values = new T[_capacity];
        }

        public ReactiveList(IReadOnlyList<T> values)
        {
            _capacity = _count.Value = values.Count;
            _values = new T[_capacity];

            for (int i = 0; i < _capacity; ++i)
                _values[i] = values[i];
        }
        #endregion

        public void Signal(int index)
        {
            Throw.IfIndexOutOfRange(index, _count);
            _changeEvent.Invoke(index, _values[index], TypeEvent.Change);
        }
        public void Signal(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
                _changeEvent.Invoke(index, _values[index], TypeEvent.Change);
        }

        public void AddOrReplace(T item)
        {
            int index = IndexOf(item);

            if (index >= 0)
            {
                _values[index] = item;
                _changeEvent.Invoke(index, item, TypeEvent.Change);
            }
            else
            {
                Add(item);
            }
        }

        #region IList
        public void Add(T item)
        {
            if (_count == _capacity)
                GrowArray();

            _values[_count] = item;
            _changeEvent.Invoke(_count, item, TypeEvent.Add);
            _count.Increment();
        }

        public void Insert(int index, T item)
        {
            Throw.IfIndexOutOfRange(index, _count);

            if (_count == _capacity)
                GrowArray();

            for (int i = _count; i > index; i--)
                _values[i] = _values[i - 1];

            _values[index] = item;
            _changeEvent.Invoke(index, item, TypeEvent.Insert);
            _count.Increment();
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
                RemoveAt(index);

            return index >= 0;
        }

        public void RemoveAt(int index)
        {
            Throw.IfIndexOutOfRange(index, _count);

            T temp = _values[index];
            
            _count.SilentRemove();
            for (int i = index; i < _count; ++i)
                _values[i] = _values[i + 1];

            _values[_count] = default;

            _changeEvent.Invoke(index, temp, TypeEvent.Remove);
            _count.Signal();
        }

        public void Clear()
        {
            for (int i = 0; i < _count; ++i)
            {
                _changeEvent.Invoke(i, _values[i], TypeEvent.Remove);
                _values[i] = default;
            }

            _count.Value = 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < _count; ++i)
                array[i] = _values[i];
        }
        #endregion

        [Impl(256)] private void GrowArray() => _values = _values.Grow(_count, _capacity = _capacity << 1 | BASE_CAPACITY);
    }
}
