using Newtonsoft.Json;

namespace Vurbiri.Reactive.Collections
{
    [JsonArray]
    public class ReactiveSet<T> : ReadOnlyReactiveSet<T> where T : class, IReactiveItem<T>
    {
        public ReactiveSet()
        {
            _values = new T[_capacity];
        }

        public ReactiveSet(int capacity)
        {
            _capacity = capacity;
            _values = new T[_capacity];
        }

        public void Add(T item)
        {
            if (_count == _capacity)
            {
                _capacity = _capacity << 1 | BASE_CAPACITY;

                T[] array = new T[_capacity];
                for (int i = 0; i < _count; i++)
                    array[i] = _values[i];
                _values = array;

                AddItem(item, _count);
                return;
            }

            for (int i = 0; i < _capacity; i++)
            {
                if (_values[i] == null)
                {
                    AddItem(item, i);
                    return;
                }
            }
        }

        public bool Remove(T item)
        {
            if (!Contains(item))
                return false;

            _values[item.Index].Removing();
            return true;
        }

        private void AddItem(T item, int index)
        {
            _values[index] = item;
            item.Adding(RedirectEvents, index);
            _count.Increment();
        }

        private void RedirectEvents(T item, TypeEvent operation)
        {
            if (operation == TypeEvent.Remove)
            {
                _values[item.Index] = null;
                _count.Decrement();
            }

            _subscriber.Invoke(item, operation);
        }
    }
}
