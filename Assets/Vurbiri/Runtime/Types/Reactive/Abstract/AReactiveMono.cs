//Assets\Vurbiri\Runtime\Types\Reactive\Abstract\AReactiveMono.cs
using System;
using UnityEngine;

namespace Vurbiri.Reactive
{
    public abstract class AReactiveMono<T> : MonoBehaviour, IReadOnlyReactive<T>
    {
        protected Action<T> actionValueChange;

        public abstract T Value { get; protected set; }

        public IUnsubscriber Subscribe(Action<T> action, bool calling = true)
        {
            actionValueChange -= action;
            actionValueChange += action;
            if (calling && action != null)
                action(Value);

            return new Unsubscriber<Action<T>>(this, action);
        }

        public void Unsubscribe(Action<T> action) => actionValueChange -= action;
    }
}
