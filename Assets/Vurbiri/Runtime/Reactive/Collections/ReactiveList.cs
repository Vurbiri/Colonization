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
        protected static readonly T[] s_empty = new T[0];

        protected T[] _values;
        protected readonly RInt _count = new(0);
        protected readonly ReactiveVersion<int, T, TypeEvent> _version = new();

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

            return _version.Add(action);
        }
        public void Unsubscribe(Action<int, T, TypeEvent> action) => _version.Remove(action);

        [Impl(256)] public bool Contains(T item) => IndexOf(item) >= 0;

        public int IndexOf(T item)
        {
            int i = _count.Value;
            while (i --> 0 && !s_comparer.Equals(_values[i], item));
            return i;
        }

        public IEnumerator<T> GetEnumerator() => _count == 0 ? EmptyEnumerator<T>.Instance : new ArrayEnumerator<T>(_values, _count, _version);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
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
                    _version.Next(index, value, TypeEvent.Change);
                }
            }
        }

        #region Constructors
        [Impl(256)] public ReactiveList() : this(BASE_CAPACITY) {}
        [Impl(256)] public ReactiveList(int capacity)
        {
            _capacity = capacity;
            _values = capacity == 0 ? s_empty : new T[capacity];
        }

        [Impl(256)] public ReactiveList(ICollection<T> values)
        {
            _capacity = _count.Value = values.Count;
            _values = new T[_capacity];
            values.CopyTo(_values, 0);
        }
        #endregion

        [Impl(256)] public void Signal(int index)
        {
            Throw.IfIndexOutOfRange(index, _count);
            _version.Signal(index, _values[index], TypeEvent.Change);
        }
        [Impl(256)] public void Signal(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
                _version.Signal(index, _values[index], TypeEvent.Change);
        }

        public void AddOrReplace(T item)
        {
            int index = IndexOf(item);

            if (index >= 0)
            {
                _values[index] = item;
                _version.Next(index, item, TypeEvent.Change);
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
            _version.Next(_count, item, TypeEvent.Add);
            _count.Increment();
        }

        public void Insert(int index, T item)
        {
            Throw.IfIndexOutOfRange(index, _count);

            if (_count == _capacity)
                GrowArray();

            if (index < _count)
                Array.Copy(_values, index, _values, index + 1, _count - index);

            _values[index] = item;
            _version.Next(index, item, TypeEvent.Insert);
            _count.Increment();
        }

        [Impl(256)]
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
            if (index < _count)
                Array.Copy(_values, index + 1, _values, index, _count - index);

            _values[_count] = default;

            _version.Next(index, temp, TypeEvent.Remove);
            _count.Signal();
        }

        public void Clear()
        {
            for (int i = 0; i < _count; ++i)
            {
                _version.Signal(i, _values[i], TypeEvent.Remove);
                _values[i] = default;
            }

            _version.Next();
            _count.Value = 0;
        }

        [Impl(256)] public void CopyTo(T[] array, int arrayIndex) => Array.Copy(_values, 0, array, arrayIndex, _count);
        #endregion

        [Impl(256)] private void GrowArray() => System.Array.Resize(ref _values, _capacity = _capacity << 1 | BASE_CAPACITY);
    }
}
