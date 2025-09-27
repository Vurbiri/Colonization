using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public abstract class Currency : ReactiveValue<int>
    {
        protected readonly VAction<int> _deltaValue = new();

        public Subscription SubscribeDelta(Action<int> action) => _deltaValue.Add(action);
    }
}
