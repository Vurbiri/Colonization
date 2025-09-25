using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    [JsonArray]
    public class ReactiveList<T> : ReadOnlyReactiveList<T>, IList<T> where T : IEquatable<T>
    {
        public bool IsReadOnly => false;

        public new T this[int index] 
        {
            get => _values[index];
            set
            {
                if (!_values[index].Equals(value))
                {
                    _values[index] = value;
                    _changeEvent.Invoke(index, value, TypeEvent.Change);
                }
            }
        }

        #region Constructors
        public ReactiveList()
        {
            _values = new T[_capacity];
        }

        public ReactiveList(int capacity)
        {
            _capacity = capacity;
            _values = new T[_capacity];
        }

        public ReactiveList(IReadOnlyList<T> values)
        {
            _capacity = _count.Value = values.Count;
            _values = new T[_capacity];

            for (int i = 0; i < _capacity; i++)
                _values[i] = values[i];
        }
        #endregion

        public void AddOrChange(T item)
        {
            int index = IndexOf(item);

            if (index >= 0)
                _changeEvent.Invoke(index, item, TypeEvent.Change);
            else
                Add(item);
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

            for (int i = _count - 1; i > index; i--)
                _values[i] = _values[i - 1];

            _values[index] = item;
            _changeEvent.Invoke(index, item, TypeEvent.Insert);
            _count.Increment();
        }
        
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if(index >= 0)
                RemoveAt(index);

            return index >= 0;
        }

        public void RemoveAt(int index)
        {
            T temp = _values[index];
            
            _count.SilentValue--;
            for (int i = index; i < _count; i++)
                _values[i] = _values[i + 1];

            _values[_count] = default;

            _changeEvent.Invoke(index, temp, TypeEvent.Remove);
            _count.Signal();
        }

        public void Clear()
        {
            for (int i = 0; i < _count; i++)
            {
                _changeEvent.Invoke(i, _values[i], TypeEvent.Remove);
                _values[i] = default;
            }

            _count.Value = 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < _count; i++)
                array[i] = _values[i];
        }
        #endregion

        private void GrowArray()
        {
            _capacity = _capacity << 1 | BASE_CAPACITY;

            T[] array = new T[_capacity];
            for(int i = 0; i < _count; i++)
                array[i] = _values[i];
            _values = array;
        }
    }
}
