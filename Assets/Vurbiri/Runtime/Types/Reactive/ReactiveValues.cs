using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Vurbiri.Reactive
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class ReactiveValues<TA, TB> : IReadOnlyReactiveValue<TA, TB>
    {
        [SerializeField, JsonProperty("v1")]
        protected TA _valueA;
        [SerializeField, JsonProperty("v2")]
        protected TB _valueB;

        protected Action<TA, TB> ActionValuesChange;

        public TA ValueA { get => _valueA; set { if (!_valueA.Equals(value)) { _valueA = value; ActionValuesChange?.Invoke(_valueA, _valueB); } } }
        public TB ValueB { get => _valueB; set { if (!_valueB.Equals(value)) { _valueB = value; ActionValuesChange?.Invoke(_valueA, _valueB); } } }

        public ReactiveValues()
        {
            _valueA = default;
            _valueB = default;
        }
        [JsonConstructor]
        public ReactiveValues(TA valueA, TB valueB)
        {
            _valueA = valueA;
            _valueB = valueB;
        }

        public Unsubscriber<TA, TB> Subscribe(Action<TA, TB> action, bool calling = true)
        {
            ActionValuesChange -= action ?? throw new ArgumentNullException("action");

            ActionValuesChange += action;
            if (calling)
                action(_valueA, _valueB);

            return new(this, action);
        }

        public void Signal() => ActionValuesChange?.Invoke(_valueA, _valueB);

        public void Unsubscribe(Action<TA, TB> action) => ActionValuesChange -= action ?? throw new ArgumentNullException("action");
    }
}
