using System;
using UnityEngine;

namespace Vurbiri.Reactive
{
    public abstract class AReactiveMono<T> : MonoBehaviour, IReactive<T>
    {
        protected Action<T> EventThisChange;

        public IUnsubscriber Subscribe(Action<T> action, bool calling = true)
        {
            EventThisChange -= action;
            EventThisChange += action;
            if (calling && action != null) 
                Callback(action);

            return new Unsubscriber<Action<T>>(this, action);
        }

        public void Unsubscribe(Action<T> action) => EventThisChange -= action;

        protected abstract void Callback(Action<T> action);
    }
}
