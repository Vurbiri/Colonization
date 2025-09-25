using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Reactive
{
    [Serializable, JsonObject(MemberSerialization.OptIn)]
    sealed public class ReactiveValues<TA, TB> : IReactiveValue<TA, TB>
    {
        [SerializeField, JsonProperty("vA")]
        private TA _valueA;
        [SerializeField, JsonProperty("vB")]
        private TB _valueB;

        private readonly VAction<TA, TB> _changeEvent = new();

        private readonly IEqualityComparer<TA> _comparerA;
        private readonly IEqualityComparer<TB> _comparerB;

        public TA ValueA { get => _valueA; set { if (!_comparerA.Equals(_valueA, value)) { _valueA = value; _changeEvent.Invoke(_valueA, _valueB); } } }
        public TB ValueB { get => _valueB; set { if (!_comparerB.Equals(_valueB, value)) { _valueB = value; _changeEvent.Invoke(_valueA, _valueB); } } }

        public ReactiveValues()
        {
            _valueA = default;
            _valueB = default;

            _comparerA = EqualityComparer<TA>.Default;
            _comparerB = EqualityComparer<TB>.Default;
        }
        
        public ReactiveValues(TA valueA, TB valueB)
        {
            _valueA = valueA;
            _valueB = valueB;

            _comparerA = EqualityComparer<TA>.Default;
            _comparerB = EqualityComparer<TB>.Default;
        }

        public Subscription Subscribe(Action<TA, TB> action, bool instantGetValue = true)
        {
            if (instantGetValue)
                action(_valueA, _valueB);

            return _changeEvent.Add(action);
        }
        public void Unsubscribe(Action<TA, TB> action) => _changeEvent.Remove(action);
        public void UnsubscribeAll() => _changeEvent.Clear();
        public void Signal() => _changeEvent.Invoke(_valueA, _valueB);

    }
}
