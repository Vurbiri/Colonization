using System;

namespace Vurbiri.Reactive.Collections
{
    public interface IReactiveElement<out T> where T : class, IReactiveElement<T>
    {
        public int Index { get; set; }
        
        public void Subscribe(Action<T, Operation> action, int index);
    }
}
