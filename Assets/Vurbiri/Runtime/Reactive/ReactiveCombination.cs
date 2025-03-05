//Assets\Vurbiri\Runtime\Types\Reactive\ReactiveCombination.cs
using System;

namespace Vurbiri.Reactive
{
    public class ReactiveCombination<TA, TB> : IReactive<TA, TB>, IDisposable
    {
        private readonly IReactiveValue<TA> _reactiveA;
        private readonly IReactiveValue<TB> _reactiveB;
        private readonly Unsubscribers _unsubscribers = new();
        private Subscriber<TA, TB> _subscriber;

        public ReactiveCombination(IReactiveValue<TA> reactiveA, IReactiveValue<TB> reactiveB)
        {
            _reactiveA = reactiveA;
            _reactiveB = reactiveB;

            _unsubscribers += reactiveA.Subscribe(value => _subscriber.Invoke(value, _reactiveB.Value), false);
            _unsubscribers += reactiveB.Subscribe(value => _subscriber.Invoke(_reactiveA.Value, value), false);
        }

        public Unsubscriber Subscribe(Action<TA, TB> action, bool calling = true)
        {
            if (calling)
                action(_reactiveA.Value, _reactiveB.Value);

            return _subscriber.Add(action);
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
            _subscriber.Dispose();
        }
    }

    public class ReactiveCombination<TA, TB, TC> : IReactive<TA, TB, TC>, IDisposable
    {
        private readonly IReactiveValue<TA> _reactiveA;
        private readonly IReactiveValue<TB> _reactiveB;
        private readonly IReactiveValue<TC> _reactiveC;
        private readonly Unsubscribers _unsubscribers = new();
        private Subscriber<TA, TB, TC> _subscriber;

        public ReactiveCombination(IReactiveValue<TA> reactiveA, IReactiveValue<TB> reactiveB, IReactiveValue<TC> reactiveC)
        {
            _reactiveA = reactiveA;
            _reactiveB = reactiveB;
            _reactiveC = reactiveC;

            _unsubscribers += reactiveA.Subscribe(value => _subscriber.Invoke(value, _reactiveB.Value, _reactiveC.Value), false);
            _unsubscribers += reactiveB.Subscribe(value => _subscriber.Invoke(_reactiveA.Value, value, _reactiveC.Value), false);
            _unsubscribers += reactiveC.Subscribe(value => _subscriber.Invoke(_reactiveA.Value, _reactiveB.Value, value), false);
        }

        public Unsubscriber Subscribe(Action<TA, TB, TC> action, bool calling = true)
        {
            if (calling)
                action(_reactiveA.Value, _reactiveB.Value, _reactiveC.Value);

            return _subscriber.Add(action);
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
            _subscriber.Dispose();
        }
    }

}
