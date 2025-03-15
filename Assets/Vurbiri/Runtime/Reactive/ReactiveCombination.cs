//Assets\Vurbiri\Runtime\Types\Reactive\ReactiveCombination.cs
using System;

namespace Vurbiri.Reactive
{
    public class ReactiveCombination<TA, TB> : IReactive<TA, TB>, IDisposable
    {
        private TA _valueA;
        private TB _valueB;
        private readonly Unsubscribers _unsubscribers = new();
        private readonly Subscriber<TA, TB> _subscriber = new();

        public ReactiveCombination(IReactiveValue<TA> reactiveA, IReactiveValue<TB> reactiveB)
        {
            _valueA = reactiveA.Value;
            _valueB = reactiveB.Value;

            _unsubscribers += reactiveA.Subscribe(value => _subscriber.Invoke(_valueA = value, _valueB), false);
            _unsubscribers += reactiveB.Subscribe(value => _subscriber.Invoke(_valueA, _valueB = value), false);
        }

        public Unsubscriber Subscribe(Action<TA, TB> action, bool calling = true)
        {
            if (calling) action(_valueA, _valueB);
            return _subscriber.Add(action);
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
            _subscriber.Dispose();
        }
    }
    //=======================================================================================
    public class ReactiveCombination<TA, TB, TC> : IReactive<TA, TB, TC>, IDisposable
    {
        private TA _valueA;
        private TB _valueB;
        private TC _valueC;
        private readonly Unsubscribers _unsubscribers = new();
        private readonly Subscriber<TA, TB, TC> _subscriber = new();

        public ReactiveCombination(IReactiveValue<TA> reactiveA, IReactiveValue<TB> reactiveB, IReactiveValue<TC> reactiveC)
        {
            _valueA = reactiveA.Value;
            _valueB = reactiveB.Value;
            _valueC = reactiveC.Value;

            _unsubscribers += reactiveA.Subscribe(value => _subscriber.Invoke(_valueA = value, _valueB, _valueC), false);
            _unsubscribers += reactiveB.Subscribe(value => _subscriber.Invoke(_valueA, _valueB = value, _valueC), false);
            _unsubscribers += reactiveC.Subscribe(value => _subscriber.Invoke(_valueA, _valueB, _valueC = value), false);
        }

        public Unsubscriber Subscribe(Action<TA, TB, TC> action, bool calling = true)
        {
            if (calling) action(_valueA, _valueB, _valueC);
            return _subscriber.Add(action);
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
            _subscriber.Dispose();
        }
    }

}
