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

        protected Action<T> EventValueChange;

        public T Value { get => _value; set { if(!_value.Equals(value)) { _value = value; EventValueChange?.Invoke(value); } } }

        public ReactiveValue() => _value = default;
        [JsonConstructor]
        public ReactiveValue(T value) => _value = value;

        public Unsubscriber<T> Subscribe(Action<T> action)
        {
            EventValueChange += action;
            action(_value);

            return new(this, action);
        }
        public Unsubscriber<T> Subscribe(Action<T> action, bool calling)
        {
            EventValueChange += action;
            if (calling) action(_value);

            return new(this, action);
        }

        public void Unsubscribe(Action<T> action) => EventValueChange -= action;

        public static implicit operator ReactiveValue<T>(T value) => new(value);
    }
}
