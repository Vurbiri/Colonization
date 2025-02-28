//Assets\Vurbiri\Runtime\Reactive\Collections\Interface\IReactiveCollections.cs
using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    public interface IListReactiveItems<out T> : IReadOnlyList<T>, IReactiveBase<Action<T, TypeEvent>> where T : class, IReactiveItem<T>
    {
        public IReactiveValue<int> CountReactive { get; }
    }

    public interface IReactiveList<out T> : IReadOnlyList<T>, IReactiveBase<Action<int, T, TypeEvent>>
    {
        public IReactiveValue<int> CountReactive { get; }
    }

    public interface IReadOnlyReactiveList<TId, out TValue> : IReadOnlyList<IReactiveValue<TValue>> where TId : IdType<TId>
    {
        public IReactiveValue<int> this[Id<TId> id] { get; }
    }
}
