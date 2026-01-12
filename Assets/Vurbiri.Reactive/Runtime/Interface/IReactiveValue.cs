namespace Vurbiri.Reactive
{
	public interface IReactiveValue< T> : IReactive<T>
	{
		public T Value { get; }
	}

	public interface IReactiveValue< TA,  TB> : IReactive<TA, TB>
	{
		public TA ValueA { get; }
		public TB ValueB { get; }
	}

	public interface IReactiveValue< TA,  TB,  TC> : IReactive<TA, TB, TC>
	{
		public TA ValueA { get; }
		public TB ValueB { get; }
		public TC ValueC { get; }
	}

	public interface IReactiveValue< TA,  TB,  TC,  TD> : IReactive<TA, TB, TC, TD>
	{
		public TA ValueA { get; }
		public TB ValueB { get; }
		public TC ValueC { get; }
		public TD ValueD { get; }
	}
}
