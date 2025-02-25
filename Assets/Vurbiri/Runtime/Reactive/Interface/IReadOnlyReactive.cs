//Assets\Vurbiri\Runtime\Types\Reactive\Interface\IReadOnlyReactive.cs
namespace Vurbiri.Reactive
{
    public interface IReadOnlyReactive<out T> : IReactive<T>
    {
        public T Value { get; }
    }

    public interface IReadOnlyReactive<out TA, out TB> : IReactive<TA, TB>
    {
        public TA ValueA { get; }
        public TB ValueB { get; }
    }

    public interface IReadOnlyReactive<out TA, out TB, out TC> : IReactive<TA, TB, TC>
    {
        public TA ValueA { get; }
        public TB ValueB { get; }
        public TC ValueC { get; }
    }
}
