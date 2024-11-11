namespace Vurbiri.Reactive.Collections
{
    using System;
    public interface IReactiveElement<T> where T : class, IReactiveElement<T>
    {
        public int Index { get; set; }
        
        public void Subscribe(Action<T, Operation> action, int index);

    }
}
