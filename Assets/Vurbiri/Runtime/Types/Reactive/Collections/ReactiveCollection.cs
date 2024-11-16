using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    public class ReactiveCollection<T> : IReactiveCollection<T> where T : class, IReactiveElement<T>
    {
        private T[] _values;
        private int _count = 0;
        private int _capacity = 4;

        private Action<T, Operation> actionCollectionChange;

        public T this[int index]
        {
            get
            {
                if (index < 0 | index >= _count)
                    throw new IndexOutOfRangeException($"index = {index}");

                return _values[index];
            }
        }

        public int Count => _count;

        #region Constructors
        public ReactiveCollection()
        {
            _values = new T[_capacity];
        }

        public ReactiveCollection(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException($"capacity = {capacity}");

            _capacity = capacity;
            _values = new T[_capacity];
        }
        #endregion

        #region IReactiveCollection
        public IUnsubscriber Subscribe(Action<T, Operation> action)
        {
            actionCollectionChange -= action ?? throw new ArgumentNullException("action");
            actionCollectionChange += action;

            for (int i = 0; i < _count; i++)
                action(_values[i], Operation.Subscribe);

            return new UnsubscriberCollection<T>(this, action);
        }

        public void Unsubscribe(Action<T, Operation> action) => actionCollectionChange -= action ?? throw new ArgumentNullException("action");
        #endregion

        public void Add(T item)
        {
            if (_count == _capacity)
                GrowArray();

            _values[_count++] = item;
            item.Subscribe(RedirectEvents, _count - 1);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
                yield return _values[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void GrowArray()
        {
            _capacity = _capacity << 1 | 4;

            T[] array = new T[_capacity];
            for (int i = 0; i < _count; i++)
                array[i] = _values[i];
            _values = array;
        }

        private void RedirectEvents(T item, Operation operation)
        {
            if (operation == Operation.Remove)
            {
                _count--;
                T temp;
                for (int i = item.Index; i < _count; i++)
                {
                    temp = _values[i + 1];
                    temp.Index = i;
                    _values[i] = temp;
                }

                _values[_count] = null;
            }

            actionCollectionChange?.Invoke(item, operation);
        }
    }
}
