using System;
using UnityEngine;

namespace Vurbiri.Reactive
{
    public abstract class AReactiveValueMono<T> : MonoBehaviour, IReactiveValue<T>
    {
        protected readonly Subscription<T> _subscriber = new();

        public abstract T Value { get; protected set; }

        public Unsubscription Subscribe(Action<T> action, bool instantGetValue = true) => _subscriber.Add(action, instantGetValue, Value);
    }
}
