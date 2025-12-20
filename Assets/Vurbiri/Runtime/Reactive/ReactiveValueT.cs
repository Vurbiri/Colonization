using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Reactive
{
    public abstract class ReactiveValue<T> : RefValue<T>, IReactiveValue<T>
    where T : struct, IEquatable<T>, IComparable<T>
    {
        protected readonly VAction<T> _onChange = new();

        [Impl(256)] public Subscription Subscribe(Action<T> action, bool instantGetValue = true) => _onChange.Add(action, _value, instantGetValue);
        [Impl(256)] public void Unsubscribe(Action<T> action) => _onChange.Remove(action);
    }
}
