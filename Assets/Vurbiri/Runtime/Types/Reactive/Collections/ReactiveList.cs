namespace Vurbiri.Reactive.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ReactiveList<T> : IList<T>, IReactiveList<T>
    {
        private T[] _values;
        private int _count = 0;
        private int _capacity = 4;

        private readonly IEqualityComparer<T> _comparer;

        private Action<int, T, Operation> actionListChange;

        public T this[int index] 
        {
            get
            {
                if (index < 0 | index >= _count)
                    throw new IndexOutOfRangeException($"index = {index}");
                
                return _values[index];
            }
            set
            {
                if (index < 0 | index >= _count)
                    throw new IndexOutOfRangeException($"index = {index}");

                _values[index] = value;

                actionListChange?.Invoke(index, value, Operation.Change);
            }
        }

        public int Count => _count;
        public bool IsReadOnly => false;

        #region Constructors
        public ReactiveList()
        {
            _comparer = EqualityComparer<T>.Default;
            _values = new T[_capacity];
        }
        public ReactiveList(IEqualityComparer<T> comparer)
        {
            _comparer = comparer ?? throw new ArgumentNullException("comparer");
            _values = new T[_capacity];
        }

        public ReactiveList(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException($"capacity = {capacity}");

            _comparer = EqualityComparer<T>.Default;
            _capacity = capacity;
            _values = new T[_capacity];
        }
        public ReactiveList(int capacity, IEqualityComparer<T> comparer)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException($"capacity = {capacity}");

            _comparer = comparer ?? throw new ArgumentNullException("comparer");
            _capacity = capacity;
            _values = new T[_capacity];
        }

        public ReactiveList(IReadOnlyList<T> values)
        {
            _comparer = EqualityComparer<T>.Default;
            _capacity = values.Count;
            _values = new T[_capacity];

            for (_count = 0; _count < _capacity; _count++)
                _values[_count] = values[_count];
        }
        public ReactiveList(IReadOnlyList<T> values, IEqualityComparer<T> comparer)
        {
            _comparer = comparer ?? throw new ArgumentNullException("comparer");
            _capacity = values.Count;
            _values = new T[_capacity];

            for (_count = 0; _count < _capacity; _count++)
                _values[_count] = values[_count];

            
        }
        #endregion

        public void ChangeSignal(int index)
        {
            if (index < 0 | index >= _count)
                throw new ArgumentOutOfRangeException($"index = {index}");

            actionListChange?.Invoke(index, _values[index], Operation.Change);
        }

        public void ChangeSignal(T item)
        {
            int index = IndexOf(item);

            if (index >= 0)
                actionListChange?.Invoke(index, _values[index], Operation.Change);
        }

        public void TryAdd(T item)
        {
            int index = IndexOf(item);

            if (index >= 0)
            {
                actionListChange?.Invoke(index, item, Operation.Change);
                return;
            }

            Add(item);
        }

        #region IReadOnlyReactiveList
        public UnsubscriberList<T> Subscribe(Action<int, T, Operation> action, bool calling = true)
        {
            actionListChange -= action ?? throw new ArgumentNullException("action");

            actionListChange += action;
            if (calling)
            {
                for (int i = 0; i < _count; i++)
                    action(i, _values[i], Operation.Init);
            }

            return new(this, action);
        }

        public void Unsubscribe(Action<int, T, Operation> action) => actionListChange -= action ?? throw new ArgumentNullException("action");
        #endregion

        #region IList
        public void Add(T item)
        {
            if (_count == _capacity)
                GrowArray();

            _values[_count] = item;
            actionListChange?.Invoke(_count, item, Operation.Add);

            _count++;
        }

        public void Insert(int index, T item)
        {
            if (index < 0 | index >= _count)
                throw new ArgumentOutOfRangeException($"index = {index}");

            if (_count == _capacity)
                GrowArray();

            for (int i = _count - 1; i > index; i--)
                _values[i] = _values[i - 1];

            _values[index] = item;
            actionListChange?.Invoke(index, item, Operation.Insert);

            _count++;
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
            if(index < 0) return false;
            RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 | index >= _count)
                throw new ArgumentOutOfRangeException($"index = {index}");
            
            T temp = _values[index];
            
            _count--;
            for (int i = index; i < _count; i++)
                _values[i] = _values[i + 1];

            _values[_count] = default;

            actionListChange?.Invoke(index, temp, Operation.Remove);
        }

        public virtual void Clear()
        {
            for (int i = 0; i < _count; i++)
            {
                actionListChange?.Invoke(i, _values[i], Operation.Remove);
                _values[i] = default;
            }

            _count = 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex < 0 | arrayIndex >= _count)
                throw new ArgumentOutOfRangeException($"arrayIndex = {arrayIndex}");

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
