//Assets\Vurbiri\Runtime\Types\Reactive\ReactiveValue.cs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Reactive
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class ReactiveValue<T> : IReactiveValue<T>
    {
        [SerializeField, JsonProperty("value")]
        protected T _value;

        protected readonly Subscriber<T> _subscriber = new();
        private readonly IEqualityComparer<T> _comparer = EqualityComparer<T>.Default;

        public T Value 
        { 
            get => _value; 
            set 
            {
                if(!_comparer.Equals(_value, value))
                { 
                    _value = value; 
                    _subscriber.Invoke(value); 
                } 
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

        public ReactiveValue(T value, IEqualityComparer<T> comparer)
        {
            _value = value;
            _comparer = comparer;
        }

        public Unsubscriber Subscribe(Action<T> action, bool calling = true)
        {
            if (calling) 
                action(_value);

            return _subscriber.Add(action);
        }

        public void Next(T value)
        {
            if (_value.Equals(value))
                return;

            _value = value; 
            _subscriber.Invoke(_value);
        }

        public void Signal() => _subscriber.Invoke(_value);

        public static implicit operator T(ReactiveValue<T> value) => value._value;
    }
}
