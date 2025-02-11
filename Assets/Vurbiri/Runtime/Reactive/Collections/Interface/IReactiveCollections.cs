//Assets\Vurbiri\Runtime\Reactive\Collections\Interface\IReactiveCollections.cs
using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    public interface IListReactiveItems<T> : IReadOnlyList<T>, IReactiveBase<Action<T, TypeEvent>> where T : class, IReactiveItem<T>
    {
    }

    public interface IReactiveList<T> : IReadOnlyList<T>, IReactiveBase<Action<int, T, TypeEvent>>
    {
    }

    public interface IReadOnlyListReactive<TId, TValue> : IReadOnlyList<IReadOnlyReactive<TValue>> where TId : IdType<TId>
    {
        public IReadOnlyReactive<int> this[Id<TId> id] { get; }
    }
}
