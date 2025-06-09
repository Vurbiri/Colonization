using System;

namespace Vurbiri.Reactive
{
    public class ReactiveCombination<TA, TB> : IReactive<TA, TB>, IDisposable
    {
        private TA _valueA;
        private TB _valueB;
        private readonly Subscription<TA, TB> _subscriber = new();
        private readonly Unsubscriptions _unsubscribers = new(2);

        public ReactiveCombination(IReactive<TA> reactiveA, IReactive<TB> reactiveB)
        {
            _unsubscribers += reactiveA.Subscribe(value => _subscriber.Invoke(_valueA = value, _valueB));
            _unsubscribers += reactiveB.Subscribe(value => _subscriber.Invoke(_valueA, _valueB = value));
        }
        public ReactiveCombination(IReactive<TA> reactiveA, IReactive<TB> reactiveB, Action<TA, TB> action) : this(reactiveA, reactiveB)
        {
            action(_valueA, _valueB);
            _subscriber.Add(action);
        }

        public Unsubscription Subscribe(Action<TA, TB> action, bool instantGetValue = true)
        {
            if (instantGetValue) action(_valueA, _valueB);
            return _subscriber.Add(action);
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
        private readonly Unsubscriptions _unsubscribers;
        private readonly Subscription<TA, TB, TC> _subscriber = new();

        public ReactiveCombination(IReactive<TA> reactiveA, IReactive<TB> reactiveB, IReactive<TC> reactiveC)
        {
            _unsubscribers = new(3);

            _unsubscribers += reactiveA.Subscribe(value => _subscriber.Invoke(_valueA = value, _valueB, _valueC));
            _unsubscribers += reactiveB.Subscribe(value => _subscriber.Invoke(_valueA, _valueB = value, _valueC));
            _unsubscribers += reactiveC.Subscribe(value => _subscriber.Invoke(_valueA, _valueB, _valueC = value));
        }
        public ReactiveCombination(IReactive<TA> reactiveA, IReactive<TB> reactiveB, IReactive<TC> reactiveC, Action<TA, TB, TC> action) :
               this(reactiveA, reactiveB, reactiveC)
        {
            action(_valueA, _valueB, _valueC);
            _subscriber.Add(action);
        }

        public ReactiveCombination(IReactive<TA, TB> reactiveAB, IReactive<TC> reactiveC)
        {
            _unsubscribers = new(2);

            _unsubscribers += reactiveAB.Subscribe((valueA, valueB) => _subscriber.Invoke(_valueA = valueA, _valueB = valueB, _valueC));
            _unsubscribers += reactiveC.Subscribe(value => _subscriber.Invoke(_valueA, _valueB, _valueC = value));
        }
        public ReactiveCombination(IReactive<TA, TB> reactiveAB, IReactive<TC> reactiveC, Action<TA, TB, TC> action) : this(reactiveAB, reactiveC)
        {
            action(_valueA, _valueB, _valueC);
            _subscriber.Add(action);
        }
        public ReactiveCombination(IReactive<TA> reactiveA, IReactive<TB, TC> reactiveBC)
        {
            _unsubscribers = new(2);

            _unsubscribers += reactiveA.Subscribe(value => _subscriber.Invoke(_valueA = value, _valueB, _valueC));
            _unsubscribers += reactiveBC.Subscribe((valueB, valueC) => _subscriber.Invoke(_valueA, _valueB = valueB, _valueC = valueC));
           
        }
        public ReactiveCombination(IReactive<TA> reactiveA, IReactive<TB, TC> reactiveBC, Action<TA, TB, TC> action) : this(reactiveA, reactiveBC)
        {
            action(_valueA, _valueB, _valueC);
            _subscriber.Add(action);
        }

        public Unsubscription Subscribe(Action<TA, TB, TC> action, bool instantGetValue = true)
        {
            if (instantGetValue) action(_valueA, _valueB, _valueC);
            return _subscriber.Add(action);
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
        }
    }
    //=======================================================================================
    public class ReactiveCombination<TA, TB, TC, TD> : IReactive<TA, TB, TC, TD>, IDisposable
    {
        private TA _valueA;
        private TB _valueB;
        private TC _valueC;
        private TD _valueD;
        private readonly Unsubscriptions _unsubscribers;
        private readonly Subscription<TA, TB, TC, TD> _subscriber = new();

        public ReactiveCombination(IReactive<TA> reactiveA, IReactive<TB> reactiveB, IReactive<TC> reactiveC, IReactive<TD> reactiveD)
        {
            _unsubscribers = new(3);

            _unsubscribers += reactiveA.Subscribe(value => _subscriber.Invoke(_valueA = value, _valueB, _valueC, _valueD));
            _unsubscribers += reactiveB.Subscribe(value => _subscriber.Invoke(_valueA, _valueB = value, _valueC, _valueD));
            _unsubscribers += reactiveC.Subscribe(value => _subscriber.Invoke(_valueA, _valueB, _valueC = value, _valueD));
            _unsubscribers += reactiveD.Subscribe(value => _subscriber.Invoke(_valueA, _valueB, _valueC, _valueD = value));
        }
        public ReactiveCombination(IReactive<TA> reactiveA, IReactive<TB> reactiveB, IReactive<TC> reactiveC, IReactive<TD> reactiveD, Action<TA, TB, TC, TD> action)
               : this(reactiveA, reactiveB, reactiveC, reactiveD)
        {
            action(_valueA, _valueB, _valueC, _valueD);
            _subscriber.Add(action);
        }

        public ReactiveCombination(IReactive<TA, TB> reactiveAB, IReactive<TC, TD> reactiveCD)
        {
            _unsubscribers = new(2);

            _unsubscribers += reactiveAB.Subscribe((valueA, valueB) => _subscriber.Invoke(_valueA = valueA, _valueB = valueB, _valueC, _valueD));
            _unsubscribers += reactiveCD.Subscribe((valueC, valueD) => _subscriber.Invoke(_valueA, _valueB, _valueC = valueC, _valueD = valueD));
        }
        public ReactiveCombination(IReactive<TA, TB> reactiveAB, IReactive<TC, TD> reactiveCD, Action<TA, TB, TC, TD> action) : this(reactiveAB, reactiveCD)
        {
            action(_valueA, _valueB, _valueC, _valueD);
            _subscriber.Add(action);
        }

        public Unsubscription Subscribe(Action<TA, TB, TC, TD> action, bool instantGetValue = true)
        {
            if (instantGetValue) action(_valueA, _valueB, _valueC, _valueD);
            return _subscriber.Add(action);
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
        }
    }
}
