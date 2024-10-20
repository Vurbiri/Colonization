namespace Vurbiri.Reactive
{
    public interface IReadOnlyReactiveValue<T> : IReactive<T>
    {
        public T Value { get; }
    }

    public interface IReadOnlyReactiveValue<TA, TB> : IReactive<TA, TB>
    {
        public TA ValueA { get; }
        public TB ValueB { get; }
    }
}
