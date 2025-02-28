//Assets\Vurbiri\Runtime\Reactive\Collections\ListReactiveItems.cs
using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    public class ListReactiveItems<T> : IListReactiveItems<T> where T : class, IReactiveItem<T>
    {
        protected T[] _values;
        protected int _capacity = 4;
        protected readonly ReactiveValue<int> _count = new(0);

        protected Subscriber<T, TypeEvent> _subscriber;

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

        public IReactiveValue<int> CountReactive => _count;

        #region Constructors
        public ListReactiveItems()
        {
            _values = new T[_capacity];
        }

        public ListReactiveItems(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException($"capacity = {capacity}");

            _capacity = capacity;
            _values = new T[_capacity];
        }
        #endregion

        #region IReactiveCollection
        public Unsubscriber Subscribe(Action<T, TypeEvent> action, bool calling = true)
        {
            if (calling)
            {
                for (int i = 0; i < _count; i++)
                    action(_values[i], TypeEvent.Subscribe);
            }

            return _subscriber.Add(action);
        }
        #endregion

        public virtual void Add(T item)
        {
            if (_count == _capacity)
                GrowArray();

            _values[_count.Value++] = item;
            item.Adding(RedirectEvents, _count - 1);
        }

        public void Remove(int index) => _values[index].Removing();
        public void Remove(T item) => _values[item.Index].Removing();

        public virtual bool Contains(T item)
        {
            for (int i = 0; i < _count; i++)
                if (_values[i].Equals(item))
                    return true;

            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
                yield return _values[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected virtual void RemoveItem(T item)
        {
            _count.SilentValue--;
            T temp;
            for (int i = item.Index; i < _count; i++)
            {
                temp = _values[i + 1];
                temp.Index = i;
                _values[i] = temp;
            }

            _values[_count] = null;
            _count.Signal();
        }

        protected void RedirectEvents(T item, TypeEvent operation)
        {
            if (operation == TypeEvent.Remove)
                RemoveItem(item);

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

    }
}
