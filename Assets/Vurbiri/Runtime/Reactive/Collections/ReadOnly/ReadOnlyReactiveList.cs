using System;
using System.Collections;
using System.Collections.Generic;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri
{
	public abstract class ReadOnlyReactiveList<T> : IReactiveList<T>, IReadOnlyList<T> where T : IEquatable<T>
    {
        protected const int BASE_CAPACITY = 7;

        protected T[] _values;
        protected int _capacity = BASE_CAPACITY;
        protected readonly RInt _count = new(0);

        protected readonly VAction<int, T, TypeEvent> _changeEvent = new();

        public T this[int index] => _values[index];

        public int Count => _count.Value;
        public IReactiveValue<int> CountReactive => _count;

        public Subscription Subscribe(Action<int, T, TypeEvent> action, bool instantGetValue = true)
        {
            if (instantGetValue)
            {
                for (int i = 0; i < _count; i++)
                    action(i, _values[i], TypeEvent.Subscribe);
            }

            return _changeEvent.Add(action);
        }

        public bool Contains(T item) => IndexOf(item) >= 0;

        public int IndexOf(T item)
        {
            int i = _count.Value;
            while (i --> 0 && !_values[i].Equals(item));
            return i;
        }

        public void Signal(int index) => _changeEvent.Invoke(index, _values[index], TypeEvent.Change);
        public void Signal(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
                _changeEvent.Invoke(index, _values[index], TypeEvent.Change);
        }

        public IEnumerator<T> GetEnumerator() => new ArrayEnumerator<T>(_values, _count.Value);
        IEnumerator IEnumerable.GetEnumerator() => new ArrayEnumerator<T>(_values, _count.Value);
    }
}
