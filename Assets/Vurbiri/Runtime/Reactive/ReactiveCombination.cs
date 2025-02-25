//Assets\Vurbiri\Runtime\Types\Reactive\ReactiveCombination.cs
using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive
{
    public class ReactiveCombination<TA, TB> : IReadOnlyReactive<TA, TB>, IDisposable
    {
        protected TA _valueA;
        protected TB _valueB;

        protected IUnsubscriber _unsubscriberA;
        protected IUnsubscriber _unsubscriberB;

        protected Subscriber<TA, TB> _subscriber = new();

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

        public IUnsubscriber Subscribe(Action<TA, TB> action, bool calling = true)
        {
            if (calling)
                action(_valueA, _valueB);

            return _subscriber.Add(action);
        }

        public void Signal() => _subscriber.Invoke(_valueA, _valueB);

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
                _subscriber.Invoke(_valueA, _valueB);
            }
        }

        private void OnChangeValueB(TB value)
        {
            if (!_comparerB.Equals(_valueB, value))
            {
                _valueB = value;
                _subscriber.Invoke(_valueA, _valueB);
            }
        }
    }
}
