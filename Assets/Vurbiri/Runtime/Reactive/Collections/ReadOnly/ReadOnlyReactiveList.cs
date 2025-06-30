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

        protected readonly Subscription<int, T, TypeEvent> _subscriber = new();

        public T this[int index] => _values[index];

        public int Count => _count.Value;
        public IReactiveValue<int> CountReactive => _count;
        public bool IsReadOnly => false;

        public Unsubscription Subscribe(Action<int, T, TypeEvent> action, bool instantGetValue = true)
        {
            if (instantGetValue)
            {
                for (int i = 0; i < _count; i++)
                    action(i, _values[i], TypeEvent.Subscribe);
            }

            return _subscriber.Add(action);
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < _count; i++)
                if (_values[i].Equals(item))
                    return true;

            return false;
        }

        public void Dispose()
        {
            for (int i = 0; i < _count; i++)
            {
                if (_values[i] is not IDisposable disposable)
                    return;

                disposable.Dispose();
            }
        }

        public IEnumerator<T> GetEnumerator() => new ArrayEnumerator<T>(_values);

        IEnumerator IEnumerable.GetEnumerator() => new ArrayEnumerator<T>(_values);
    }
}
