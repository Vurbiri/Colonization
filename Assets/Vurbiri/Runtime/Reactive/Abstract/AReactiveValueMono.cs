//Assets\Vurbiri\Runtime\Types\Reactive\Abstract\AReactiveValueMono.cs
using System;
using UnityEngine;

namespace Vurbiri.Reactive
{
    public abstract class AReactiveValueMono<T> : MonoBehaviour, IReactiveValue<T>
    {
        protected readonly Signer<T> _signer = new();

        public abstract T Value { get; protected set; }

        public Unsubscriber Subscribe(Action<T> action, bool instantGetValue = true) => _signer.Add(action, instantGetValue, Value);
    }
}
