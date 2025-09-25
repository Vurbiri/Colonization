using System;
using UnityEngine;

namespace Vurbiri.Reactive
{
    public abstract class AReactiveValueMono<T> : MonoBehaviour, IReactiveValue<T> where T : IEquatable<T>
    {
        [SerializeField] protected T _value;

        protected readonly VAction<T> _changeEvent = new();

        public virtual T Value
        {
            get => _value;
            set
            {
                if (!_value.Equals(value))
                    _changeEvent.Invoke(_value = value);
            }
        }

        public Subscription Subscribe(Action<T> action, bool instantGetValue = true) => _changeEvent.Add(action, instantGetValue, _value);
    }
}
