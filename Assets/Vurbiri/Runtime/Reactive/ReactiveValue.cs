//Assets\Vurbiri\Runtime\Types\Reactive\ReactiveValue.cs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Reactive
{
    [Serializable, JsonObject(MemberSerialization.OptIn)]
    sealed public class ReactiveValue<T> : IReactiveValue<T>
    {
        [SerializeField, JsonProperty("value")]
        private T _value;

        private readonly Subscriber<T> _subscriber = new();
        private readonly IEqualityComparer<T> _comparer = EqualityComparer<T>.Default;

        public T Value 
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _value; 
            set 
            {
                if(!_comparer.Equals(_value, value))
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

        public ReactiveValue(T value, IEqualityComparer<T> comparer)
        {
            Errors.ThrowIfNull(comparer);

            _value = value;
            _comparer = comparer;
        }

        public Unsubscriber Subscribe(Action<T> action, bool calling = true) => _subscriber.Add(action, calling, _value);

        public void Signal() => _subscriber.Invoke(_value);

        public void Dispose() => _subscriber.Dispose();

        public static explicit operator ReactiveValue<T>(T value) => new(value);
        public static implicit operator T(ReactiveValue<T> value) => value._value;
    }
}
