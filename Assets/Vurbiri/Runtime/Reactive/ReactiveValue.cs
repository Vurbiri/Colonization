using System;
using UnityEngine;

namespace Vurbiri.Reactive
{
    [Serializable]
    public class ReactiveValue<T> : IReactiveValue<T> where T : IEquatable<T>
    {
        [SerializeField] protected T _value;

        protected readonly VAction<T> _subscriber = new();

        public T Value 
        {
            get => _value; 
            set 
            {
                if(!_value.Equals(value))
                    _subscriber.Invoke(_value = value);
            } 
        }

        public T SilentValue { get => _value; set => _value = value; }

        public ReactiveValue()
        {
            _value = default;
        }
        
        public ReactiveValue(T value)
        {
            _value = value;
        }

        public Subscription Subscribe(Action<T> action, bool instantGetValue = true) => _subscriber.Add(action, instantGetValue, _value);
        public void Unsubscribe(Action<T> action) => _subscriber.Remove(action);
        public void UnsubscribeAll() => _subscriber.Clear();
        public void Signal() => _subscriber.Invoke(_value);


        public static implicit operator ReactiveValue<T>(T value) => new(value);
        public static implicit operator T(ReactiveValue<T> value) => value._value;
    }
}
