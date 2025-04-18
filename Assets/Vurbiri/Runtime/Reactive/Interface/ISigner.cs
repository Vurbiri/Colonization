//Assets\Vurbiri\Runtime\Reactive\Interface\ISigner.cs
using System;

namespace Vurbiri.Reactive
{
    public interface IUnsubscribed<in TDelegate> where TDelegate : Delegate
    {
        public void Remove(TDelegate action);
    }
    //=======================================================================================

    public interface ISigner : IUnsubscribed<Action>
    {
        public Unsubscriber Add(Action action);

        public static Unsubscriber operator +(ISigner subscriber, Action action) => subscriber.Add(action);
        public static ISigner operator -(ISigner subscriber, Action action)
        {
            subscriber.Remove(action);
            return subscriber;
        }
    }
    //=======================================================================================

    public interface ISigner<out T> : IUnsubscribed<Action<T>>
    {
        public Unsubscriber Add(Action<T> action);

        public static Unsubscriber operator +(ISigner<T> subscriber, Action<T> action) => subscriber.Add(action);
        public static ISigner<T> operator -(ISigner<T> subscriber, Action<T> action)
        {
            subscriber.Remove(action);
            return subscriber;
        }
    }
    //=======================================================================================

    public interface ISigner<out TA, out TB> : IUnsubscribed<Action<TA, TB>>
    {
        public Unsubscriber Add(Action<TA, TB> action);

        public static Unsubscriber operator +(ISigner<TA, TB> subscriber, Action<TA, TB> action) => subscriber.Add(action);
        public static ISigner<TA, TB> operator -(ISigner<TA, TB> subscriber, Action<TA, TB> action)
        {
            subscriber.Remove(action);
            return subscriber;
        }
    }
    //=======================================================================================

    public interface ISigner<out TA, out TB, out TC> : IUnsubscribed<Action<TA, TB, TC>>
    {
        public Unsubscriber Add(Action<TA, TB, TC> action);

        public static Unsubscriber operator +(ISigner<TA, TB, TC> subscriber, Action<TA, TB, TC> action) => subscriber.Add(action);
        public static ISigner<TA, TB, TC> operator -(ISigner<TA, TB, TC> subscriber, Action<TA, TB, TC> action)
        {
            subscriber.Remove(action);
            return subscriber;
        }
    }
    //=======================================================================================

    public interface ISigner<out TA, out TB,out TC,out TD> : IUnsubscribed<Action<TA, TB, TC, TD>>
    {
        public Unsubscriber Add(Action<TA, TB, TC, TD> action);

        public static Unsubscriber operator +(ISigner<TA, TB, TC, TD> subscriber, Action<TA, TB, TC, TD> action) => subscriber.Add(action);
        public static ISigner<TA, TB, TC, TD> operator -(ISigner<TA, TB, TC, TD> subscriber, Action<TA, TB, TC, TD> action)
        {
            subscriber.Remove(action);
            return subscriber;
        }
    }
}
