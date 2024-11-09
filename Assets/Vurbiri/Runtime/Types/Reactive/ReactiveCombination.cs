using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive
{
    public class ReactiveCombination<TA, TB> : IReadOnlyReactiveValue<TA, TB>, IDisposable
    {
        protected TA _valueA;
        protected TB _valueB;

        protected Unsubscriber<TA> _unsubscriberA;
        protected Unsubscriber<TB> _unsubscriberB;

        protected Action<TA, TB> ActionValuesChange;

        private readonly IEqualityComparer<TA> _comparerA;
        private readonly IEqualityComparer<TB> _comparerB;

        public TA ValueA => _valueA;
        public TB ValueB => _valueB;

        public ReactiveCombination(IReactive<TA> reactiveA, IReactive<TB> reactiveB)
        {
            _comparerA = EqualityComparer<TA>.Default;
            _comparerB = EqualityComparer<TB>.Default;

            _unsubscriberA = reactiveA.Subscribe(OnChangeValueA);
            _unsubscriberB = reactiveB.Subscribe(OnChangeValueB);
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

        public void Dispose()
        {
            _unsubscriberA?.Unsubscribe();
            _unsubscriberB?.Unsubscribe();
        }

        private void OnChangeValueA(TA value)
        {
            if (!_comparerA.Equals(_valueA, value))
            {
                _valueA = value;
                ActionValuesChange?.Invoke(_valueA, _valueB);
            }
        }

        private void OnChangeValueB(TB value)
        {
            if (!_comparerB.Equals(_valueB, value))
            {
                _valueB = value;
                ActionValuesChange?.Invoke(_valueA, _valueB);
            }
        }
    }
}
