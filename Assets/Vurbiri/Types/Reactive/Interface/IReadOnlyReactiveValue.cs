namespace Vurbiri.Reactive
{
    public interface IReadOnlyReactiveValue<T> : IReactive<T>
    {
        public T Value { get; }
    }
}
