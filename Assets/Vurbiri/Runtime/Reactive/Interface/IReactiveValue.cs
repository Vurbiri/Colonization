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

    public interface IReactiveValue<out TA, out TB, out TC, out TD> : IReactive<TA, TB, TC, TD>
    {
        public TA ValueA { get; }
        public TB ValueB { get; }
        public TC ValueC { get; }
        public TD ValueD { get; }
    }
}
