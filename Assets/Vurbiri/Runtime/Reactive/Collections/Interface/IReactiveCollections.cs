using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    public interface IReactiveSet<out T> : IReadOnlyCollection<T>, IReactiveBase<Action<T, TypeEvent>> where T : class, IReactiveItem<T>
    {
        public IReactiveValue<int> CountReactive { get; }
    }

    public interface IReactiveList<out T> : IReadOnlyList<T>, IReactiveBase<Action<int, T, TypeEvent>>
    {
        public IReactiveValue<int> CountReactive { get; }
    }
}
