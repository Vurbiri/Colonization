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

        private readonly Signer<T> _signer = new();
        private readonly IEqualityComparer<T> _comparer = EqualityComparer<T>.Default;

        public T Value 
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _value; 
            set 
            {
                if(!_comparer.Equals(_value, value))
                    _signer.Invoke(_value = value);
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
            Throw.IfNull(comparer);

            _value = value;
            _comparer = comparer;
        }

        public Unsubscriber Subscribe(Action<T> action, bool sendCallback = true) => _signer.Add(action, sendCallback, _value);

        public void Signal() => _signer.Invoke(_value);


        public static explicit operator ReactiveValue<T>(T value) => new(value);
        public static implicit operator T(ReactiveValue<T> value) => value._value;
    }
}
