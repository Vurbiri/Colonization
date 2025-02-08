//Assets\Vurbiri\Runtime\Types\Reactive\Collections\Interface\IReactiveElement.cs
using System;

namespace Vurbiri.Reactive.Collections
{
    public interface IReactiveElement<T> : IReactive<T, TypeEvent>, IEquatable<T> where T : class, IReactiveElement<T>
    {
        public int Index { get; set; }
        
        public void Adding(Action<T, TypeEvent> action, int index);
        public void Removing();
    }
}
