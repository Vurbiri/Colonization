//Assets\Vurbiri\Runtime\Reactive\Collections\Interface\IReactiveItem.cs
using System;

namespace Vurbiri.Reactive.Collections
{
    public interface IReactiveItem<T> : IReactive<T, TypeEvent>, IEquatable<T> where T : class, IReactiveItem<T>
    {
        public int Index { get; set; }
        
        public void Adding(Action<T, TypeEvent> action, int index);
        public void Removing();
    }
}
