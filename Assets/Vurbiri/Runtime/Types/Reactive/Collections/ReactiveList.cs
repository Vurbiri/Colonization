using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    public class ReactiveList<T> : IList<T>, IReadOnlyReactiveList<T>
    {
        private T[] _values;
        private int _count = 0;
        private int _capacity = 4;

        private Action<int, T, Operation> ActionListChange;

        public T this[int index] 
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException($"index = {index}");
                
                return _values[index];
            }
            set
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException($"index = {index}");

                _values[index] = value;

                ActionListChange?.Invoke(index, value, Operation.Change);
            }
        }

        public int Count => _count;

        public bool IsReadOnly => false;

        public ReactiveList() => _values = new T[_capacity];

        public ReactiveList(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException($"capacity = {capacity}");

            _capacity = capacity;
            _values = new T[_capacity];
        }

        public ReactiveList(IReadOnlyList<T> values)
        {
            _capacity = values.Count;
            _values = new T[_capacity];

            for (_count = 0; _count < _capacity; _count++)
                _values[_count] = values[_count];
        }

        public void ChangeSignal(int index)
        {
            if (index < 0 || index >= _count)
                throw new ArgumentOutOfRangeException($"index = {index}");

            ActionListChange?.Invoke(index, _values[index], Operation.Change);
        }

        public void ChangeSignal(T item)
        {
            int index = IndexOf(item);

            if (index >= 0)
                ActionListChange?.Invoke(index, _values[index], Operation.Change);
        }

        public void TryAdd(T item)
        {
            int index = IndexOf(item);

            if (index >= 0)
            {
                _values[index] = item;
                ActionListChange?.Invoke(index, item, Operation.Change);
                return;
            }

            Add(item);
        }

        #region IReadOnlyReactiveList
        public UnsubscriberList<T> Subscribe(Action<int, T, Operation> action, bool calling = true)
        {
            ActionListChange -= action ?? throw new ArgumentNullException("action");

            ActionListChange += action;
            if (calling)
            {
                for (int i = 0; i < _count; i++)
                    action(i, _values[i], Operation.Init);
            }

            return new(this, action);
        }

        public void Unsubscribe(Action<int, T, Operation> action) => ActionListChange -= action ?? throw new ArgumentNullException("action");
        #endregion

        #region IList
        public void Add(T item)
        {
            _values[_count] = item;

            ActionListChange?.Invoke(_count, item, Operation.Add);

            if (++_count == _capacity)
                ResizeArray();
        }

        public void Clear()
        {
            for (int i = 0; i < _count; i++)
            {
                ActionListChange?.Invoke(i, _values[i], Operation.Remove);
                _values[i] = default;
            }

            _count = 0;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < _count; i++)
                if(_values[i].Equals(item))
                    return true;

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex < 0 || arrayIndex >= _count)
                throw new ArgumentOutOfRangeException($"arrayIndex = {arrayIndex}");

            for (int i = arrayIndex; i < _count; i++)
                array[i] = _values[i];
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
                 yield return _values[i];
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < _count; i++)
                if (_values[i].Equals(item))
                    return i;

            return -1;
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index >= _count)
                throw new ArgumentOutOfRangeException($"index = {index}");

            for (int i = _count - 1; i > index; i--)
                _values[i] = _values[i - 1];
            
            _values[index] = item;
            ActionListChange?.Invoke(index, item, Operation.Insert);

            if (++_count == _capacity)
                ResizeArray();
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
            if (index < 0 || index >= _count)
                throw new ArgumentOutOfRangeException($"index = {index}");

            _count--;

            ActionListChange?.Invoke(index, _values[index], Operation.Remove);

            if (index == _count)
            {
                _values[index] = default;
                return;
            }

            for (int i = index; i < _count; i++)
                _values[i] = _values[i + 1];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        private void ResizeArray()
        {
            _capacity <<= 1 + 4;

            T[] array = new T[_capacity];
            for(int i = 0; i < _count; i++)
                array[i] = _values[i];
            _values = array;
        }
    }
}
