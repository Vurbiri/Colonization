//Assets\Vurbiri\Runtime\Types\Reactive\ReactiveValue.cs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Reactive
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class ReactiveValue<T> : IReadOnlyReactive<T>
    {
        [SerializeField, JsonProperty("value")]
        protected T _value;

        protected Action<T> actionValueChange;
        private readonly IEqualityComparer<T> _comparer = EqualityComparer<T>.Default;

        public T Value 
        { 
            get => _value; 
            set 
            {
                if(!_comparer.Equals(_value, value))
                { 
                    _value = value; 
                    actionValueChange?.Invoke(value); 
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

        public IUnsubscriber Subscribe(Action<T> action, bool calling = true)
        {
            actionValueChange -= action ?? throw new ArgumentNullException("action");
            actionValueChange += action;
            if (calling) 
                action(_value);

            return new Unsubscriber<Action<T>>(this, action);
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
    }
}
