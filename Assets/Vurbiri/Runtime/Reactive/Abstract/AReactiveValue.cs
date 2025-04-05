//Assets\Vurbiri\Runtime\Reactive\Abstract\AReactiveValue.cs
using System;

namespace Vurbiri.Reactive
{
    public abstract class AReactiveValue<T> : IReactiveValue<T>
    {
        protected readonly Signer<T> _signer = new();

        public abstract T Value { get; protected set; }

        public Unsubscriber Subscribe(Action<T> action, bool instantGetValue = true) => _signer.Add(action, instantGetValue, Value);
    }
}
