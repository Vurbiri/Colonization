using System;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    sealed public class Blood : ACurrency
    {
        private readonly Ability _max;

        public ReactiveValue<int> Max { [Impl(256)] get => _max; }
        public int Percent { [Impl(256)] get => (100 * _value) / _max; }

        public Blood(int value, Ability maxValue, System.Action<int> action)
        {
            _value = value;
            _max = maxValue;
            _onChange.Add(action);
        }

        [Impl(256)] public void Add(int delta) => Set(_value + delta);
        [Impl(256)] public void Remove(int delta) => Set(_value - delta);

        public void Set(int value)
        {
            value = Math.Min(value, _max.Value);

            Throw.IfNegative(value);

            int delta = value - _value;
            if (delta != 0)
            {
                _value = value;

                _onChange.Invoke(value);
                _deltaValue.Invoke(delta);
            }
        }
    }
}
