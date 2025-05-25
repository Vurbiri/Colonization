using System;

namespace Vurbiri.Reactive
{
    public abstract class AReactiveValue<T> : IReactiveValue<T> where T : IEquatable<T>
    {
        protected T _value;

        protected readonly Subscription<T> _subscriber = new();

        public virtual T Value
        {
            get => _value;
            set
            {
                if (!_value.Equals(value))
                    _subscriber.Invoke(_value = value);
            }
        }

        public Unsubscription Subscribe(Action<T> action, bool instantGetValue = true) => _subscriber.Add(action, instantGetValue, _value);
    }
}
