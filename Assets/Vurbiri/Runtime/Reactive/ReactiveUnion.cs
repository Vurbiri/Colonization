//Assets\Vurbiri\Runtime\Reactive\ReactiveUnion.cs
using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive
{
    public class ReactiveIntUnion : IReactiveValue<int>, IDisposable
    {
		private readonly List<IReactiveValue<int>> _reactive;
        private Unsubscribers _unsubscribers = new();
        private Subscriber<int> _subscriber;
        private int _count;

        public int Value
        {
            get
            {
                int sum = 0;
                for (int i = 0; i < _count; i++)
                    sum += _reactive[i].Value;

                return sum;
            }
        }

        public ReactiveIntUnion()
        {
            _reactive = new();
            _count = 0;
        }

        public ReactiveIntUnion(int capacity)
        {
            _reactive = new(capacity);
            _count = 0;
        }

        public ReactiveIntUnion(IReadOnlyList<IReactiveValue<int>> reactive)
        {
            _reactive = new(reactive);
            _count = _reactive.Count;

            for(int i = 0; i < _count; i++)
                _unsubscribers += _reactive[i].Subscribe(v => _subscriber.Invoke(Value), false);
        }

        public void Add(IReactiveValue<int> reactive)
        {
            _reactive.Add(reactive);
            _count = _reactive.Count;

            _unsubscribers += reactive.Subscribe(v => _subscriber.Invoke(Value), false);
        }
        
        public Unsubscriber Subscribe(Action<int> action, bool calling = true)
        {
            if (calling)
                action(Value);

            return _subscriber.Add(action);
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
            _subscriber.Dispose();
        }
    }
}
