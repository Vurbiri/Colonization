using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Vurbiri.Reactive
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class ReactiveValue<T> : IReadOnlyReactiveValue<T>
    {
        [SerializeField, JsonProperty("value")]
        protected T _value;

        protected Action<T> actionValueChange;

        public T Value { get => _value; set { if(!_value.Equals(value)) { _value = value; actionValueChange?.Invoke(_value); } } }

        public ReactiveValue() => _value = default;
        [JsonConstructor]
        public ReactiveValue(T value) => _value = value;

        public Unsubscriber<T> Subscribe(Action<T> action, bool calling = true)
        {
            actionValueChange -= action ?? throw new ArgumentNullException("action");
            actionValueChange += action;
            if (calling) 
                action(_value);

            return new(this, action);
        }

        public void Next(T value)
        {
            if (_value.Equals(value))
                return;

            _value = value; 
            actionValueChange?.Invoke(_value);
        }

        public void Signal() => actionValueChange?.Invoke(_value);

        public void Unsubscribe(Action<T> action) => actionValueChange -= action ?? throw new ArgumentNullException("action");

        public static implicit operator ReactiveValue<T>(T value) => new(value);
    }
}
