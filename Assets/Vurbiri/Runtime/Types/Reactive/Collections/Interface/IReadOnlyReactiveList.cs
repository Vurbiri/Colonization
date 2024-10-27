using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    public interface IReadOnlyReactiveList<T> : IReactiveList<T>, IReadOnlyList<T>
    {
        
    }
}
