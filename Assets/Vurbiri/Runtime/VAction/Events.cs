using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	#region ============== EventExt ======================
	public static class EventExt
	{
		[Impl(256)] public static Events<TA, TB> Combine<TA, TB>(this Event<TA> eventA, Event<TB> eventB) => new(eventA, eventB);

		[Impl(256)] public static Events<TA, TB, TC> Combine<TA, TB, TC>(this Event<TA> eventA, Event<TB> eventB, Event<TC> eventC) => new(eventA, eventB, eventC);
		[Impl(256)] public static Events<TA, TB, TC> Combine<TA, TB, TC>(this Event<TA, TB> eventAB, Event<TC> eventC) => new(eventAB, eventC);
		[Impl(256)] public static Events<TA, TB, TC> Combine<TA, TB, TC>(this Event<TC> eventC, Event<TA, TB> eventAB) => new(eventAB, eventC);
	}
	#endregion

	#region ============== Events<TA, TB> ======================
	public class Events<TA, TB> : Event<TA, TB>, IDisposable
	{
        private readonly Subscription _subscription;
        private TA _valueA;
		private TB _valueB;

		public TA ValueA { [Impl(256)] get => _valueA; }
		public TB ValueB { [Impl(256)] get => _valueB; }

		public Events(Event<TA> eventA, Event<TB> eventB)
		{
			_subscription  = eventA.Add(value => _action(_valueA = value, _valueB));
			_subscription += eventB.Add(value => _action(_valueA, _valueB = value));
			_action = Dummy.Action;
		}
		public Events(Event<TA> eventA, Event<TB> eventB, Action<TA, TB> action) : this(eventA, eventB)
		{
			action(_valueA, _valueB);
			_action = action;
		}

		[Impl(256)] public void Signal() => _action(_valueA, _valueB);

		[Impl(256)] public void Dispose()
		{
			_subscription.Dispose();
			_action = Dummy.Action;
		}
	}
	#endregion

	#region ============== Events<TA, TB, TC> ======================
	public class Events<TA, TB, TC> : Event<TA, TB, TC>, IDisposable
	{
        private readonly Subscription _subscription;
        private TA _valueA;
		private TB _valueB;
		private TC _valueC;

		public TA ValueA { [Impl(256)] get => _valueA; }
		public TB ValueB { [Impl(256)] get => _valueB; }
		public TC ValueC { [Impl(256)] get => _valueC; }

		public Events(Event<TA> eventA, Event<TB> eventB, Event<TC> eventC)
		{
			_subscription  = eventA.Add(value => _action(_valueA = value, _valueB, _valueC));
			_subscription += eventB.Add(value => _action(_valueA, _valueB = value, _valueC));
			_subscription += eventC.Add(value => _action(_valueA, _valueB, _valueC = value));
			_action = Dummy.Action;
		}
		public Events(Event<TA> eventA, Event<TB> eventB, Event<TC> eventC, Action<TA, TB, TC> action) : this(eventA, eventB, eventC)
		{
			action(_valueA, _valueB, _valueC);
			_action = action;
		}

		public Events(Event<TA, TB> eventAB, Event<TC> eventC)
		{
			_subscription = eventAB.Add((valueA, valueB) => _action(_valueA = valueA, _valueB = valueB, _valueC));
			_subscription += eventC.Add(value => _action(_valueA, _valueB, _valueC = value));
			_action = Dummy.Action;
		}
		public Events(Event<TA, TB> eventAB, Event<TC> eventC, Action<TA, TB, TC> action) : this(eventAB, eventC)
		{
			action(_valueA, _valueB, _valueC);
			_action = action;
		}

		[Impl(256)] public Events(Event<TC> eventC, Event<TA, TB> eventAB) : this(eventAB, eventC) { }
		[Impl(256)] public Events(Event<TC> eventC, Event<TA, TB> eventAB, Action<TA, TB, TC> action) : this(eventAB, eventC, action) { }

		[Impl(256)] public void Signal() => _action(_valueA, _valueB, _valueC);

		[Impl(256)] public void Dispose()
		{
			_subscription.Dispose();
			_action = Dummy.Action;
		}
	}
	#endregion
}
