//Assets\Vurbiri\Runtime\Types\Reactive\Abstract\AReactiveMono.cs
using System;
using UnityEngine;

namespace Vurbiri.Reactive
{
    public abstract class AReactiveMono<T> : MonoBehaviour, IReadOnlyReactive<T>
    {
        protected Subscriber<T> _subscriber = new();

        public abstract T Value { get; protected set; }

        public Unsubscriber Subscribe(Action<T> action, bool calling = true)
        {
            if (calling)
                action(Value);

            return _subscriber.Add(action);
        }

    }
}
