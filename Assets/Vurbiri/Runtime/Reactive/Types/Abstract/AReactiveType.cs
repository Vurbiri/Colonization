using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Reactive
{
    public abstract class AReactiveType<T> : ReactiveValue<T>  where T : struct, IEquatable<T>, IComparable<T>
    {
        public new T Value
        {
            [Impl(256)] get => _value;
            [Impl(256)] set { if (!_value.Equals(value)) _changeEvent.Invoke(_value = value); }
        }

        public T SilentValue
        {
            [Impl(256)] get => _value;
            [Impl(256)] set => _value = value;
        }

        [Impl(256)] protected AReactiveType(T value) => _value = value;

        public void UnsubscribeAll() => _changeEvent.Clear();
        public void Signal() => _changeEvent.Invoke(_value);
    }
}
