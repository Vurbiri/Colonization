using System;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public abstract class ACurrency : ReactiveValue<int>
    {
        protected readonly VAction<int> _deltaValue = new();

        [Impl(256)] public Subscription SubscribeDelta(Action<int> action) => _deltaValue.Add(action);

        [Impl(256)] public static int operator +(ACurrency a, ACurrency b) => a._value + b._value;
        [Impl(256)] public static int operator +(ACurrency currency, int value) => currency._value + value;
        [Impl(256)] public static int operator +(int value, ACurrency currency) => value + currency._value;

        [Impl(256)] public static int operator -(ACurrency a, ACurrency b) => a._value - b._value;
        [Impl(256)] public static int operator -(ACurrency currency, int value) => currency._value - value;
        [Impl(256)] public static int operator -(int value, ACurrency currency) => value - currency._value;

        [Impl(256)] public static int operator *(ACurrency a, ACurrency b) => a._value * b._value;
        [Impl(256)] public static int operator *(ACurrency currency, int value) => currency._value * value;
        [Impl(256)] public static int operator *(int value, ACurrency currency) => value * currency._value;
    }
}
