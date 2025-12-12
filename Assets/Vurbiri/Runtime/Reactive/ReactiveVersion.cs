using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	//=======================================================================================
	public class ReactiveVersion : Version, IUnsubscribed<Action>
	{
		private Action _action;

		[Impl(256)] public ReactiveVersion() => _action = Dummy.Action;

        [Impl(256)] public void SilentNext() => ++_version;
        [Impl(256)] public new void Next()
		{
			++_version;
			_action.Invoke();
		}

        [Impl(256)] public void Signal() => _action.Invoke();

        [Impl(256)] public Subscription Add(Action action)
		{
			_action += action;
			return Subscription.Create(this, action);
		}
		[Impl(256)] public void Remove(Action action) => _action -= action;

		[Impl(256)] public void Clear() => _action = Dummy.Action;
	}
	//=======================================================================================
	public class ReactiveVersion<TA> : Version, IUnsubscribed<Action<TA>>
	{
		private Action<TA> _action;

		[Impl(256)] public ReactiveVersion() => _action = Dummy.Action;

        [Impl(256)] public void Next(TA a)
		{
			++_version;
			_action.Invoke(a);
		}

        [Impl(256)] public void Signal(TA a) => _action.Invoke(a);

        [Impl(256)] public Subscription Add(Action<TA> action)
		{
			_action += action;
			return Subscription.Create(this, action);
		}
		[Impl(256)] public void Remove(Action<TA> action) => _action -= action;

		[Impl(256)] public void Clear() => _action = Dummy.Action;
	}
	//=======================================================================================
	public class ReactiveVersion<TA, TB> : Version, IUnsubscribed<Action<TA, TB>>
	{
		private Action<TA, TB> _action;

		[Impl(256)] public ReactiveVersion() => _action = Dummy.Action;

        [Impl(256)] public void Next(TA a, TB b)
		{
			++_version;
			_action.Invoke(a, b);
		}

        [Impl(256)] public void Signal(TA a, TB b) => _action.Invoke(a, b);

        [Impl(256)] public Subscription Add(Action<TA, TB> action)
		{
			_action += action;
			return Subscription.Create(this, action);
		}
		[Impl(256)] public void Remove(Action<TA, TB> action) => _action -= action;

		[Impl(256)] public void Clear() => _action = Dummy.Action;
	}
	//=======================================================================================
	public class ReactiveVersion<TA, TB, TC> : Version, IUnsubscribed<Action<TA, TB, TC>>
	{
		private Action<TA, TB, TC> _action;

		[Impl(256)] public ReactiveVersion() => _action = Dummy.Action;

        [Impl(256)] public void Next(TA a, TB b, TC c)
		{
			++_version;
			_action.Invoke(a, b, c);
		}

        [Impl(256)] public void Signal(TA a, TB b, TC c) => _action.Invoke(a, b, c);

        [Impl(256)] public Subscription Add(Action<TA, TB, TC> action)
		{
			_action += action;
			return Subscription.Create(this, action);
		}
		[Impl(256)] public void Remove(Action<TA, TB, TC> action) => _action -= action;

		[Impl(256)] public void Clear() => _action = Dummy.Action;
	}
	//=======================================================================================
	public class ReactiveVersion<TA, TB, TC, TD> : Version, IUnsubscribed<Action<TA, TB, TC, TD>>
	{
		private Action<TA, TB, TC, TD> _action;

		[Impl(256)] public ReactiveVersion() => _action = Dummy.Action;

		[Impl(256)] public void Next(TA a, TB b, TC c, TD d)
		{
			++_version;
			_action.Invoke(a, b, c, d);
		}

        [Impl(256)] public void Signal(TA a, TB b, TC c, TD d) => _action.Invoke(a, b, c, d);

        [Impl(256)] public Subscription Add(Action<TA, TB, TC, TD> action)
		{
			_action += action;
			return Subscription.Create(this, action);
		}
		[Impl(256)] public void Remove(Action<TA, TB, TC, TD> action) => _action -= action;
		[Impl(256)] public void Clear() => _action = Dummy.Action;
	}
	//=======================================================================================
}
