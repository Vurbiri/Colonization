//Assets\Vurbiri\Runtime\Reactive\Interface\ISubscriber.cs
using System;

namespace Vurbiri.Reactive
{
    public interface IUnsubscribed<in TDelegate> where TDelegate : Delegate
    {
        public void Remove(TDelegate action);
    }
    //=======================================================================================

    public interface ISubscription : IUnsubscribed<Action>
    {
        public Unsubscription Add(Action action);

        public static Unsubscription operator +(ISubscription subscriber, Action action) => subscriber.Add(action);
        public static ISubscription operator -(ISubscription subscriber, Action action)
        {
            subscriber.Remove(action);
            return subscriber;
        }
    }
    //=======================================================================================

    public interface ISubscription<out T> : IUnsubscribed<Action<T>>
    {
        public Unsubscription Add(Action<T> action);

        public static Unsubscription operator +(ISubscription<T> subscriber, Action<T> action) => subscriber.Add(action);
        public static ISubscription<T> operator -(ISubscription<T> subscriber, Action<T> action)
        {
            subscriber.Remove(action);
            return subscriber;
        }
    }
    //=======================================================================================

    public interface ISubscription<out TA, out TB> : IUnsubscribed<Action<TA, TB>>
    {
        public Unsubscription Add(Action<TA, TB> action);

        public static Unsubscription operator +(ISubscription<TA, TB> subscriber, Action<TA, TB> action) => subscriber.Add(action);
        public static ISubscription<TA, TB> operator -(ISubscription<TA, TB> subscriber, Action<TA, TB> action)
        {
            subscriber.Remove(action);
            return subscriber;
        }
    }
    //=======================================================================================

    public interface ISubscription<out TA, out TB, out TC> : IUnsubscribed<Action<TA, TB, TC>>
    {
        public Unsubscription Add(Action<TA, TB, TC> action);

        public static Unsubscription operator +(ISubscription<TA, TB, TC> subscriber, Action<TA, TB, TC> action) => subscriber.Add(action);
        public static ISubscription<TA, TB, TC> operator -(ISubscription<TA, TB, TC> subscriber, Action<TA, TB, TC> action)
        {
            subscriber.Remove(action);
            return subscriber;
        }
    }
    //=======================================================================================

    public interface ISubscription<out TA, out TB,out TC,out TD> : IUnsubscribed<Action<TA, TB, TC, TD>>
    {
        public Unsubscription Add(Action<TA, TB, TC, TD> action);

        public static Unsubscription operator +(ISubscription<TA, TB, TC, TD> subscriber, Action<TA, TB, TC, TD> action) => subscriber.Add(action);
        public static ISubscription<TA, TB, TC, TD> operator -(ISubscription<TA, TB, TC, TD> subscriber, Action<TA, TB, TC, TD> action)
        {
            subscriber.Remove(action);
            return subscriber;
        }
    }
}
