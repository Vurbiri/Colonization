using System;

namespace Vurbiri.Reactive.Collections
{
    public interface IReactiveItem<out T> : IReactive<T, TypeEvent>, IDisposable where T : class, IReactiveItem<T>
    {
        public int Index { get; set; }
        
        public void Adding(Action<T, TypeEvent> action, int index);
        public void Removing();
    }
}
