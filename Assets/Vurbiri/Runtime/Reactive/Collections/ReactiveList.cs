//Assets\Vurbiri\Runtime\Types\Reactive\Collections\ReactiveList.cs
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vurbiri.Reactive.Collections
{
    [JsonArray]
    public class ReactiveList<T> : IList<T>, IReactiveList<T>
    {
        private T[] _values;
        private int _capacity = 4;
        protected readonly RInt _count = new(0);
        
        private readonly IEqualityComparer<T> _comparer;

        private readonly Subscription<int, T, TypeEvent> _subscriber = new();

        public T this[int index] 
        {
            get => _values [index];
            set
            {
                _values[index] = value;
                _subscriber.Invoke(index, value, TypeEvent.Change);
            }
        }

        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _count;
        }
        public IReactiveValue<int> CountReactive => _count;
        public bool IsReadOnly => false;

        #region Constructors
        public ReactiveList()
        {
            _comparer = EqualityComparer<T>.Default;
            _values = new T[_capacity];
        }
        public ReactiveList(IEqualityComparer<T> comparer)
        {
            Throw.IfNull(comparer);

            _comparer = comparer;
            _values = new T[_capacity];
        }

        public ReactiveList(int capacity)
        {
            _comparer = EqualityComparer<T>.Default;
            _capacity = capacity;
            _values = new T[_capacity];
        }
        public ReactiveList(int capacity, IEqualityComparer<T> comparer)
        {
            Throw.IfNull(comparer);

            _comparer = comparer;
            _capacity = capacity;
            _values = new T[_capacity];
        }

        public ReactiveList(IReadOnlyList<T> values)
        {
            _comparer = EqualityComparer<T>.Default;
            _capacity = _count.Value = values.Count;
            _values = new T[_capacity];

            for (int i = 0; i < _capacity; i++)
                _values[i] = values[i];
        }
        public ReactiveList(IReadOnlyList<T> values, IEqualityComparer<T> comparer)
        {
            Throw.IfNull(comparer);

            _comparer = comparer;
            _capacity = _count.Value = values.Count;
            _values = new T[_capacity];

            for (int i = 0; i < _capacity; i++)
                _values[i] = values[i];
        }
        #endregion

        public void Signal(int index) => _subscriber.Invoke(index, _values[index], TypeEvent.Change);

        public void Signal(T item)
        {
            int index = IndexOf(item);

            if (index >= 0)
                _subscriber.Invoke(index, _values[index], TypeEvent.Change);
        }

        public void AddOrChange(T item)
        {
            int index = IndexOf(item);

            if (index >= 0)
            {
                _subscriber.Invoke(index, item, TypeEvent.Change);
                return;
            }

            Add(item);
        }

        #region IReadOnlyReactiveList
        public Unsubscription Subscribe(Action<int, T, TypeEvent> action, bool instantGetValue = true)
        {
            if (instantGetValue)
            {
                for (int i = 0; i < _count; i++)
                    action(i, _values[i], TypeEvent.Subscribe);
            }

            return _subscriber.Add(action);
        }
        #endregion

        #region IList
        public void Add(T item)
        {
            if (_count == _capacity)
                GrowArray();

            _values[_count] = item;
            _subscriber.Invoke(_count, item, TypeEvent.Add);

            _count.Value++;
        }

        public void Insert(int index, T item)
        {
            Throw.IfIndexOutOfRange(index, _count);

            if (_count == _capacity)
                GrowArray();

            for (int i = _count - 1; i > index; i--)
                _values[i] = _values[i - 1];

            _values[index] = item;
            _subscriber.Invoke(index, item, TypeEvent.Insert);

            _count.Value++;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < _count; i++)
                if (_comparer.Equals(_values[i], item))
                    return true;

            return false;
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < _count; i++)
                if (_comparer.Equals(_values[i], item))
                    return i;

            return -1;
        }
        
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if(index < 0) 
                return false;

            RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            T temp = _values[index];
            
            _count.SilentValue--;
            for (int i = index; i < _count; i++)
                _values[i] = _values[i + 1];

            _values[_count] = default;

            _subscriber.Invoke(index, temp, TypeEvent.Remove);
            _count.Signal();
        }

        public void Clear()
        {
            for (int i = 0; i < _count; i++)
            {
                _subscriber.Invoke(i, _values[i], TypeEvent.Remove);
                _values[i] = default;
            }

            _count.Value = 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < _count; i++)
                array[i] = _values[i];
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
                yield return _values[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        private void GrowArray()
        {
            _capacity = _capacity << 1 | 4;

            T[] array = new T[_capacity];
            for(int i = 0; i < _count; i++)
                array[i] = _values[i];
            _values = array;
        }
    }
}
