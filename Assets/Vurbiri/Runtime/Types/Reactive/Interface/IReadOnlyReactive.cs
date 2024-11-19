namespace Vurbiri.Reactive
{
    public interface IReadOnlyReactive<T> : IReactive<T>
    {
        public T Value { get; }
    }

    public interface IReadOnlyReactive<TA, TB> : IReactive<TA, TB>
    {
        public TA ValueA { get; }
        public TB ValueB { get; }
    }
}
