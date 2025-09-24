using System;

namespace Vurbiri.Reactive
{
    public interface IUnsubscribed<in TDelegate> where TDelegate : Delegate
    {
        public void Remove(TDelegate action);
    }

    public class Unsubscription : IDisposable
    {
        private Action _action;

        private Unsubscription(Action action) => _action = action;

        internal static Unsubscription Create<T>(IUnsubscribed<T> subscriber, T action) where T : Delegate
        {
            var unsub = new Unsubscribe<T>(subscriber, action);
            return new(unsub.Invoke);
        }

        public void Dispose()
        {
            _action(); 
            _action = Dummy;
        }

        public static Unsubscription operator +(Unsubscription a, Unsubscription b) => new((a?._action + b?._action) ?? Dummy);
        public static Unsubscription operator ^(Unsubscription a, Unsubscription b)
        {
            a?._action();
            return b;
        }

        private static void Dummy() { }

        // ******************** Nested ********************************
        private class Unsubscribe<TDelegate> where TDelegate : Delegate
        {
            private WeakReference<IUnsubscribed<TDelegate>> _weakSigner;
            private TDelegate _action;

            public Unsubscribe(IUnsubscribed<TDelegate> subscriber, TDelegate action)
            {
                _weakSigner = new(subscriber);
                _action = action;
            }

            public void Invoke()
            {
                if (_weakSigner != null && _weakSigner.TryGetTarget(out IUnsubscribed<TDelegate> signer))
                {
                    signer.Remove(_action);

                    _weakSigner.SetTarget(null);
                    _weakSigner = null;
                }
                _action = null;
            }
        }
        // ************************************************************
    }
}
