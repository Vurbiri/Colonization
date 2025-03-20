//Assets\Vurbiri\Runtime\Types\Reactive\Unsubscriber.cs
using System;

namespace Vurbiri.Reactive
{
    public abstract class Unsubscriber
    {
        public abstract void Unsubscribe();
    }


    sealed internal class Unsubscriber<TDelegate> : Unsubscriber where TDelegate : Delegate
    {
        private WeakReference<IUnsubscribed<TDelegate>> _weakSubscriber;
        private TDelegate action;

        public Unsubscriber(IUnsubscribed<TDelegate> subscriber, TDelegate action)
        {
            _weakSubscriber = new(subscriber);
            this.action = action;
        }

        public override void Unsubscribe()
        {
            if(_weakSubscriber != null && _weakSubscriber.TryGetTarget(out IUnsubscribed<TDelegate> subscriber))
            {
                subscriber.Remove(action);

                _weakSubscriber.SetTarget(null);
                _weakSubscriber = null;
            }
            action = null;
        }
    }
}
