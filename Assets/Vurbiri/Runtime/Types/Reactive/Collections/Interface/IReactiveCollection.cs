//Assets\Vurbiri\Runtime\Types\Reactive\Collections\Interface\IReactiveCollection.cs
using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    public interface IReactiveCollection<T> : IReadOnlyList<T>, IReactiveBase<Action<T, TypeEvent>> where T : class, IReactiveElement<T>
    {
    }

    public interface IReactiveList<T> : IReadOnlyList<T>, IReactiveBase<Action<int, T, TypeEvent>>
    {
    }
}
