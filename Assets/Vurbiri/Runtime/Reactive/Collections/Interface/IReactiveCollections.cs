//Assets\Vurbiri\Runtime\Reactive\Collections\Interface\IReactiveCollections.cs
using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    public interface IListReactiveItems<out T> : IReadOnlyList<T>, IReactiveBase<Action<T, TypeEvent>> where T : class, IReactiveItem<T>
    {
        public IReadOnlyReactive<int> CountReactive { get; }
    }

    public interface IReactiveList<out T> : IReadOnlyList<T>, IReactiveBase<Action<int, T, TypeEvent>>
    {
        public IReadOnlyReactive<int> CountReactive { get; }
    }

    public interface IReadOnlyReactiveList<TId, out TValue> : IReadOnlyList<IReadOnlyReactive<TValue>> where TId : IdType<TId>
    {
        public IReadOnlyReactive<int> this[Id<TId> id] { get; }
    }
}
