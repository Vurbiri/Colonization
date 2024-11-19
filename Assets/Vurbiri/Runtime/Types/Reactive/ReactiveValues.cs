using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Reactive
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class ReactiveValues<TA, TB> : IReadOnlyReactive<TA, TB>
    {
        [SerializeField, JsonProperty("v1")]
        protected TA _valueA;
        [SerializeField, JsonProperty("v2")]
        protected TB _valueB;

        protected Action<TA, TB> ActionValuesChange;

        private readonly IEqualityComparer<TA> _comparerA;
        private readonly IEqualityComparer<TB> _comparerB;

        public TA ValueA { get => _valueA; set { if (!_comparerA.Equals(_valueA, value)) { _valueA = value; ActionValuesChange?.Invoke(_valueA, _valueB); } } }
        public TB ValueB { get => _valueB; set { if (!_comparerB.Equals(_valueB, value)) { _valueB = value; ActionValuesChange?.Invoke(_valueA, _valueB); } } }

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

        public IUnsubscriber Subscribe(Action<TA, TB> action, bool calling = true)
        {
            ActionValuesChange -= action ?? throw new ArgumentNullException("action");

            ActionValuesChange += action;
            if (calling)
                action(_valueA, _valueB);

            return new Unsubscriber<Action<TA, TB>>(this, action);
        }

        public void Signal() => ActionValuesChange?.Invoke(_valueA, _valueB);

        public void Unsubscribe(Action<TA, TB> action) => ActionValuesChange -= action ?? throw new ArgumentNullException("action");
    }
}
