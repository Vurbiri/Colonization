//Assets\Vurbiri\Runtime\Reactive\Interface\ISubscriber.cs
using System;

namespace Vurbiri.Reactive
{
    public interface IUnsubscribed<TDelegate> where TDelegate : Delegate
    {
        public void Remove(TDelegate action);
    }
    //=======================================================================================

    public interface ISubscriber : IUnsubscribed<Action>
    {
        public Unsubscriber Add(Action action);

        public static Unsubscriber operator +(ISubscriber subscriber, Action action) => subscriber.Add(action);
        public static ISubscriber operator -(ISubscriber subscriber, Action action)
        {
            subscriber.Remove(action);
            return subscriber;
        }
    }
    //=======================================================================================

    public interface ISubscriber<T> : IUnsubscribed<Action<T>>
    {
        public Unsubscriber Add(Action<T> action);

        public static Unsubscriber operator +(ISubscriber<T> subscriber, Action<T> action) => subscriber.Add(action);
        public static ISubscriber<T> operator -(ISubscriber<T> subscriber, Action<T> action)
        {
            subscriber.Remove(action);
            return subscriber;
        }
    }
    //=======================================================================================

    public interface ISubscriber<TA, TB> : IUnsubscribed<Action<TA, TB>>
    {
        public Unsubscriber Add(Action<TA, TB> action);

        public static Unsubscriber operator +(ISubscriber<TA, TB> subscriber, Action<TA, TB> action) => subscriber.Add(action);
        public static ISubscriber<TA, TB> operator -(ISubscriber<TA, TB> subscriber, Action<TA, TB> action)
        {
            subscriber.Remove(action);
            return subscriber;
        }
    }
    //=======================================================================================

    public interface ISubscriber<TA, TB, TC> : IUnsubscribed<Action<TA, TB, TC>>
    {
        public Unsubscriber Add(Action<TA, TB, TC> action);

        public static Unsubscriber operator +(ISubscriber<TA, TB, TC> subscriber, Action<TA, TB, TC> action) => subscriber.Add(action);
        public static ISubscriber<TA, TB, TC> operator -(ISubscriber<TA, TB, TC> subscriber, Action<TA, TB, TC> action)
        {
            subscriber.Remove(action);
            return subscriber;
        }
    }
    //=======================================================================================

    public interface ISubscriber<TA, TB, TC, TD> : IUnsubscribed<Action<TA, TB, TC, TD>>
    {
        public Unsubscriber Add(Action<TA, TB, TC, TD> action);

        public static Unsubscriber operator +(ISubscriber<TA, TB, TC, TD> subscriber, Action<TA, TB, TC, TD> action) => subscriber.Add(action);
        public static ISubscriber<TA, TB, TC, TD> operator -(ISubscriber<TA, TB, TC, TD> subscriber, Action<TA, TB, TC, TD> action)
        {
            subscriber.Remove(action);
            return subscriber;
        }
    }
}
