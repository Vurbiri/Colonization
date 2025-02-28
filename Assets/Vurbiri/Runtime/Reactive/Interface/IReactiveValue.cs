//Assets\Vurbiri\Runtime\Types\Reactive\Interface\IReactiveValue.cs
namespace Vurbiri.Reactive
{
    public interface IReactiveValue<out T> : IReactive<T>
    {
        public T Value { get; }
    }

    public interface IReactiveValue<out TA, out TB> : IReactive<TA, TB>
    {
        public TA ValueA { get; }
        public TB ValueB { get; }
    }

    public interface IReactiveValue<out TA, out TB, out TC> : IReactive<TA, TB, TC>
    {
        public TA ValueA { get; }
        public TB ValueB { get; }
        public TC ValueC { get; }
    }
}
