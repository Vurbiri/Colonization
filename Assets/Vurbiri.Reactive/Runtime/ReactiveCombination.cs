using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Reactive
{
	public interface ICombination : IDisposable
	{
		public void Signal();
	}

	#region ============== ReactiveCombination<TA, TB> ==============================
	public class ReactiveCombination<TA, TB> : IReactiveValue<TA, TB>, ICombination
	{
		private TA _valueA;
		private TB _valueB;
		private readonly Subscription _subscription;
		private readonly VAction<TA, TB> _action = new();

		public TA ValueA { [Impl(256)] get => _valueA; }
		public TB ValueB { [Impl(256)] get => _valueB; }

		public ReactiveCombination(IReactive<TA> reactiveA, IReactive<TB> reactiveB)
		{
			_subscription  = reactiveA.Subscribe(value => _action.Invoke(_valueA = value, _valueB));
			_subscription += reactiveB.Subscribe(value => _action.Invoke(_valueA, _valueB = value));
		}
		public ReactiveCombination(IReactive<TA> reactiveA, IReactive<TB> reactiveB, Action<TA, TB> action) : this(reactiveA, reactiveB)
		{
			action(_valueA, _valueB);
			_action.Add(action);
		}

		[Impl(256)] public Subscription Subscribe(Action<TA, TB> action, bool instantGetValue = true)
		{
			if (instantGetValue) action(_valueA, _valueB);
			return _action.Add(action);
		}

		[Impl(256)] public void Signal() => _action.Invoke(_valueA, _valueB);

		[Impl(256)] public void Dispose()
		{
			_subscription.Dispose();
			_action.Clear();
		}
	}
	#endregion

	#region ============== ReactiveCombination<TA, TB, TC> ==========================
	public class ReactiveCombination<TA, TB, TC> : IReactiveValue<TA, TB, TC>, ICombination
	{
		private TA _valueA;
		private TB _valueB;
		private TC _valueC;
		private readonly Subscription _subscription;
		private readonly VAction<TA, TB, TC> _action = new();

		public TA ValueA { [Impl(256)] get => _valueA; }
		public TB ValueB { [Impl(256)] get => _valueB; }
		public TC ValueC { [Impl(256)] get => _valueC; }

		public ReactiveCombination(IReactive<TA> reactiveA, IReactive<TB> reactiveB, IReactive<TC> reactiveC)
		{
			_subscription  = reactiveA.Subscribe(value => _action.Invoke(_valueA = value, _valueB, _valueC));
			_subscription += reactiveB.Subscribe(value => _action.Invoke(_valueA, _valueB = value, _valueC));
			_subscription += reactiveC.Subscribe(value => _action.Invoke(_valueA, _valueB, _valueC = value));
		}
		public ReactiveCombination(IReactive<TA> reactiveA, IReactive<TB> reactiveB, IReactive<TC> reactiveC, Action<TA, TB, TC> action) :
			   this(reactiveA, reactiveB, reactiveC)
		{
			action(_valueA, _valueB, _valueC);
			_action.Add(action);
		}

		public ReactiveCombination(IReactive<TA, TB> reactiveAB, IReactive<TC> reactiveC)
		{
			_subscription  = reactiveAB.Subscribe((valueA, valueB) => _action.Invoke(_valueA = valueA, _valueB = valueB, _valueC));
			_subscription += reactiveC.Subscribe(value => _action.Invoke(_valueA, _valueB, _valueC = value));
		}
		public ReactiveCombination(IReactive<TA, TB> reactiveAB, IReactive<TC> reactiveC, Action<TA, TB, TC> action) : this(reactiveAB, reactiveC)
		{
			action(_valueA, _valueB, _valueC);
			_action.Add(action);
		}

        [Impl(256)] public ReactiveCombination(IReactive<TC> reactiveC, IReactive<TA, TB> reactiveAB) : this(reactiveAB, reactiveC) { }
        [Impl(256)] public ReactiveCombination(IReactive<TC> reactiveC, IReactive<TA, TB> reactiveAB, Action<TA, TB, TC> action) : this(reactiveAB, reactiveC, action) { }

        [Impl(256)] public Subscription Subscribe(Action<TA, TB, TC> action, bool instantGetValue = true)
		{
			if (instantGetValue) action(_valueA, _valueB, _valueC);
			return _action.Add(action);
		}

		[Impl(256)] public void Signal() => _action.Invoke(_valueA, _valueB, _valueC);

		[Impl(256)] public void Dispose()
		{
			_subscription.Dispose();
			_action.Clear();
		}
	}
	#endregion

	#region ============== ReactiveCombination<TA, TB, TC, TD> ======================
	public class ReactiveCombination<TA, TB, TC, TD> : IReactiveValue<TA, TB, TC, TD>, ICombination
	{
		private TA _valueA;
		private TB _valueB;
		private TC _valueC;
		private TD _valueD;
		private readonly Subscription _subscription;
		private readonly VAction<TA, TB, TC, TD> _action = new();

		public TA ValueA { [Impl(256)] get => _valueA; }
		public TB ValueB { [Impl(256)] get => _valueB; }
		public TC ValueC { [Impl(256)] get => _valueC; }
		public TD ValueD { [Impl(256)] get => _valueD; }

		public ReactiveCombination(IReactive<TA> reactiveA, IReactive<TB> reactiveB, IReactive<TC> reactiveC, IReactive<TD> reactiveD)
		{
			_subscription  = reactiveA.Subscribe(value => _action.Invoke(_valueA = value, _valueB, _valueC, _valueD));
			_subscription += reactiveB.Subscribe(value => _action.Invoke(_valueA, _valueB = value, _valueC, _valueD));
			_subscription += reactiveC.Subscribe(value => _action.Invoke(_valueA, _valueB, _valueC = value, _valueD));
			_subscription += reactiveD.Subscribe(value => _action.Invoke(_valueA, _valueB, _valueC, _valueD = value));
		}
		public ReactiveCombination(IReactive<TA> reactiveA, IReactive<TB> reactiveB, IReactive<TC> reactiveC, IReactive<TD> reactiveD, Action<TA, TB, TC, TD> action)
			   : this(reactiveA, reactiveB, reactiveC, reactiveD)
		{
			action(_valueA, _valueB, _valueC, _valueD);
			_action.Add(action);
		}

		public ReactiveCombination(IReactive<TA, TB> reactiveAB, IReactive<TC, TD> reactiveCD)
		{
			_subscription  = reactiveAB.Subscribe((valueA, valueB) => _action.Invoke(_valueA = valueA, _valueB = valueB, _valueC, _valueD));
			_subscription += reactiveCD.Subscribe((valueC, valueD) => _action.Invoke(_valueA, _valueB, _valueC = valueC, _valueD = valueD));
		}
		public ReactiveCombination(IReactive<TA, TB> reactiveAB, IReactive<TC, TD> reactiveCD, Action<TA, TB, TC, TD> action) : this(reactiveAB, reactiveCD)
		{
			action(_valueA, _valueB, _valueC, _valueD);
			_action.Add(action);
		}

		public ReactiveCombination(IReactive<TA, TB, TC> reactiveABC, IReactive<TD> reactiveD)
		{
			_subscription  = reactiveABC.Subscribe((valueA, valueB, valueC) => _action.Invoke(_valueA = valueA, _valueB = valueB, _valueC = valueC, _valueD));
			_subscription += reactiveD.Subscribe(value => _action.Invoke(_valueA, _valueB, _valueC, _valueD = value));
		}
		public ReactiveCombination(IReactive<TA, TB, TC> reactiveABC, IReactive<TD> reactiveD, Action<TA, TB, TC, TD> action) : this(reactiveABC, reactiveD)
		{
			action(_valueA, _valueB, _valueC, _valueD);
			_action.Add(action);
		}

        [Impl(256)] public ReactiveCombination(IReactive<TD> reactiveD, IReactive<TA, TB, TC> reactiveABC) : this(reactiveABC, reactiveD) { }
        [Impl(256)] public ReactiveCombination(IReactive<TD> reactiveD, IReactive<TA, TB, TC> reactiveABC, Action<TA, TB, TC, TD> action) : this(reactiveABC, reactiveD, action) { }

        [Impl(256)] public Subscription Subscribe(Action<TA, TB, TC, TD> action, bool instantGetValue = true)
		{
			if (instantGetValue) action(_valueA, _valueB, _valueC, _valueD);
			return _action.Add(action);
		}

		[Impl(256)] public void Signal() => _action.Invoke(_valueA, _valueB, _valueC, _valueD);

		[Impl(256)] public void Dispose()
		{
			_subscription.Dispose();
			_action.Clear();
		}
	}
	#endregion
}
