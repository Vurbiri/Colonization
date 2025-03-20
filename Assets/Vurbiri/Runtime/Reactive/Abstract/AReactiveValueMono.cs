//Assets\Vurbiri\Runtime\Types\Reactive\Abstract\AReactiveValueMono.cs
using System;
using UnityEngine;

namespace Vurbiri.Reactive
{
    public abstract class AReactiveValueMono<T> : MonoBehaviour, IReactiveValue<T>
    {
        protected readonly Subscriber<T> _subscriber = new();

        public abstract T Value { get; protected set; }

        public Unsubscriber Subscribe(Action<T> action, bool calling = true) => _subscriber.Add(action, calling, Value);

        public virtual void Dispose() => _subscriber.Dispose();
    }
}
