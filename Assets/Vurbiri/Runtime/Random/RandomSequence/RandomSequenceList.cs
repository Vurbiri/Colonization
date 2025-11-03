using System;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public class RandomSequenceList<T> : RandomSequence<T>
    {
        private const int BASE_CAPACITY = 3;

        [Impl(256)] public RandomSequenceList() : base(BASE_CAPACITY) { }
        [Impl(256)] public RandomSequenceList(int capacity) : base(capacity) { }
        [Impl(256)] public RandomSequenceList(IReadOnlyList<T> ids) : base(ids) { }
        [Impl(256)] public RandomSequenceList(params T[] ids) : base(ids) { }

        public void Add(T item)
        {
            if (_count == _capacity)
            {
                _capacity = _capacity << 1 | BASE_CAPACITY;

                var array = new T[_capacity];
                for (int i = 0; i < _count; i++)
                    array[i] = _values[i];
                _values = array;
            }

            _values[_count++] = item;
        }

        public void Remove(T item)
        {
            int index = -1;
            while (++index < _count && !_values[index].Equals(item)) ;

            RemoveAt(index);
        }

        public void Remove<U>(U item) where U : IEquatable<T> 
        {
            int index = -1;
            while (++index < _count && !item.Equals(_values[index])) ;

            RemoveAt(index);
        }

        private void RemoveAt(int index)
        {
            if (index < _count)
            {
                _count--;
                for (; index < _count; index++)
                    _values[index] = _values[index + 1];

                if (_cursor >= index)
                    _cursor--;
            }
        }
    }
}
