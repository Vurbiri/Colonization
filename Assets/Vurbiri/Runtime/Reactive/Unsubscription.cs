//Assets\Vurbiri\Runtime\Types\Reactive\Unsubscriber.cs
using System;

namespace Vurbiri.Reactive
{
    public abstract class Unsubscription
    {
        public abstract void Unsubscribe();

        public static Unsubscription operator +(Unsubscription a, Unsubscription b)
        {
            a?.Unsubscribe();
            return b;
        }
    }


    sealed internal class Unsubscription<TDelegate> : Unsubscription where TDelegate : Delegate
    {
        private WeakReference<IUnsubscribed<TDelegate>> _weakSigner;
        private TDelegate action;

        public Unsubscription(IUnsubscribed<TDelegate> subscriber, TDelegate action)
        {
            _weakSigner = new(subscriber);
            this.action = action;
        }

        public override void Unsubscribe()
        {
            if(_weakSigner != null && _weakSigner.TryGetTarget(out IUnsubscribed<TDelegate> signer))
            {
                signer.Remove(action);

                _weakSigner.SetTarget(null);
                _weakSigner = null;
            }
            action = null;
        }
    }
}
