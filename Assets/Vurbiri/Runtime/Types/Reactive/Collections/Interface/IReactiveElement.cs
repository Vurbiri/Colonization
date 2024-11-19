using System;

namespace Vurbiri.Reactive.Collections
{
    public interface IReactiveElement<T> where T : class, IReactiveElement<T>
    {
        public int Index { get; set; }
        
        public void Subscribe(Action<T, TypeEvent> action, int index);
    }
}
