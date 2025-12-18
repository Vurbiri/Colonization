using System;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public class RandomSequenceList<T> : RandomSequence<T>
    {
        private const int BASE_CAPACITY = 3;
        private static readonly IEqualityComparer<T> s_comparer = EqualityComparer<T>.Default;
        private int _capacity;

        [Impl(256)] public RandomSequenceList() : this(BASE_CAPACITY) { }
        [Impl(256)] public RandomSequenceList(int capacity) : base(capacity)
        {
            _capacity = capacity;
            _count = 0;
        }
        [Impl(256)] public RandomSequenceList(IReadOnlyList<T> ids) : base(ids) => _capacity = _count;
        [Impl(256)] public RandomSequenceList(params T[] ids) : base(ids) => _capacity = _count;

        public void Add(T item)
        {
            if (_count == _capacity)
            {
                _capacity = _capacity << 1 | BASE_CAPACITY;
                Array.Resize(ref _values, _capacity);
            }

            _values[_count++] = item;
        }

        public void Remove(T item)
        {
            int index = _count;
            while (index --> 0 && !s_comparer.Equals(_values[index], item));

            if(index >= 0)
            {
                if (index < --_count)
                    Array.Copy(_values, index + 1, _values, index, _count - index);

                _values[_count] = default;
                if (_cursor >= index)
                    --_cursor;
            }
        }
    }
}
