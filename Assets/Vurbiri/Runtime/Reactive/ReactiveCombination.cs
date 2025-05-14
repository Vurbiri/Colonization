//Assets\Vurbiri\Runtime\Types\Reactive\ReactiveCombination.cs
using System;

namespace Vurbiri.Reactive
{
    public class ReactiveCombination<TA, TB> : IReactive<TA, TB>, IDisposable
    {
        private TA _valueA;
        private TB _valueB;
        private readonly Signer<TA, TB> _signer = new();
        private readonly Unsubscribers _unsubscribers = new(2);

        public ReactiveCombination(IReactiveValue<TA> reactiveA, IReactiveValue<TB> reactiveB)
        {
            _valueA = reactiveA.Value;
            _valueB = reactiveB.Value;

            _unsubscribers += reactiveA.Subscribe(value => _signer.Invoke(_valueA = value, _valueB), false);
            _unsubscribers += reactiveB.Subscribe(value => _signer.Invoke(_valueA, _valueB = value), false);
        }
        public ReactiveCombination(IReactive<TA> reactiveA, IReactive<TB> reactiveB)
        {
            _unsubscribers += reactiveA.Subscribe(value => _signer.Invoke(_valueA = value, _valueB));
            _unsubscribers += reactiveB.Subscribe(value => _signer.Invoke(_valueA, _valueB = value));
        }

        public ReactiveCombination(IReactiveValue<TA> reactiveA, IReactiveValue<TB> reactiveB, Action<TA, TB> action) : this(reactiveA, reactiveB)
        {
            action(_valueA, _valueB);
            _signer.Add(action);
        }
        public ReactiveCombination(IReactive<TA> reactiveA, IReactive<TB> reactiveB, Action<TA, TB> action) : this(reactiveA, reactiveB)
        {
            action(_valueA, _valueB);
            _signer.Add(action);
        }

        public Unsubscriber Subscribe(Action<TA, TB> action, bool instantGetValue = true)
        {
            if (instantGetValue) action(_valueA, _valueB);
            return _signer.Add(action);
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
        }
    }
    //=======================================================================================
    public class ReactiveCombination<TA, TB, TC> : IReactive<TA, TB, TC>, IDisposable
    {
        private TA _valueA;
        private TB _valueB;
        private TC _valueC;
        private readonly Unsubscribers _unsubscribers;
        private readonly Signer<TA, TB, TC> _signer = new();

        public ReactiveCombination(IReactiveValue<TA> reactiveA, IReactiveValue<TB> reactiveB, IReactiveValue<TC> reactiveC)
        {
            _unsubscribers = new(3);

            _valueA = reactiveA.Value;
            _valueB = reactiveB.Value;
            _valueC = reactiveC.Value;

            _unsubscribers += reactiveA.Subscribe(value => _signer.Invoke(_valueA = value, _valueB, _valueC), false);
            _unsubscribers += reactiveB.Subscribe(value => _signer.Invoke(_valueA, _valueB = value, _valueC), false);
            _unsubscribers += reactiveC.Subscribe(value => _signer.Invoke(_valueA, _valueB, _valueC = value), false);
        }
        public ReactiveCombination(IReactive<TA> reactiveA, IReactive<TB> reactiveB, IReactive<TC> reactiveC)
        {
            _unsubscribers = new(3);

            _unsubscribers += reactiveA.Subscribe(value => _signer.Invoke(_valueA = value, _valueB, _valueC));
            _unsubscribers += reactiveB.Subscribe(value => _signer.Invoke(_valueA, _valueB = value, _valueC));
            _unsubscribers += reactiveC.Subscribe(value => _signer.Invoke(_valueA, _valueB, _valueC = value));
        }

        public ReactiveCombination(IReactiveValue<TA, TB> reactiveAB, IReactiveValue<TC> reactiveC)
        {
            _unsubscribers = new(2);

            _valueA = reactiveAB.ValueA;
            _valueB = reactiveAB.ValueB;
            _valueC = reactiveC.Value;

            _unsubscribers += reactiveAB.Subscribe((valueA, valueB) => _signer.Invoke(_valueA = valueA, _valueB = valueB, _valueC), false);
            _unsubscribers += reactiveC.Subscribe(value => _signer.Invoke(_valueA, _valueB, _valueC = value), false);
        }
        public ReactiveCombination(IReactive<TA, TB> reactiveAB, IReactive<TC> reactiveC)
        {
            _unsubscribers = new(2);

            _unsubscribers += reactiveAB.Subscribe((valueA, valueB) => _signer.Invoke(_valueA = valueA, _valueB = valueB, _valueC));
            _unsubscribers += reactiveC.Subscribe(value => _signer.Invoke(_valueA, _valueB, _valueC = value));
        }

        public Unsubscriber Subscribe(Action<TA, TB, TC> action, bool instantGetValue = true)
        {
            if (instantGetValue) action(_valueA, _valueB, _valueC);
            return _signer.Add(action);
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
        }
    }

}
