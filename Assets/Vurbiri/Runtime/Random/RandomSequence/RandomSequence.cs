using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public class RandomSequence : RandomSequence<int>
    {
        [Impl(256)] public RandomSequence(int count) : this(0, count) { }
        public RandomSequence(int min, int max) : base(max - min)
        {
            _count = _capacity;

            _values[0] = min;
            for (int i = 1, j; i < _count; i++)
            {
                _values[i] = i + min;

                j = SysRandom.Next(i);
                (_values[j], _values[i]) = (_values[i], _values[j]);
            }
        }

        [Impl(256)] public RandomSequence(IReadOnlyList<int> ids) : base(ids) { }
        [Impl(256)] public RandomSequence(params int[] ids) : base(ids) { }
    }
}
