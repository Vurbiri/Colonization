using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Reactive.Collections
{
	//=======================================================================================
	public class ReactiveVersion : Vurbiri.Collections.Version, IUnsubscribed<Action>
	{
		private Action _action;

		[Impl(256)] public ReactiveVersion() => _action = Dummy.Action;

        [Impl(256)] public void SilentNext() =>  _version = unchecked(_version + 1);
        [Impl(256)] public new void Next()
		{
            _version = unchecked(_version + 1);
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
	public class ReactiveVersion<TA> : Vurbiri.Collections.Version, IUnsubscribed<Action<TA>>
	{
		private Action<TA> _action;

		[Impl(256)] public ReactiveVersion() => _action = Dummy.Action;

        [Impl(256)] public void Next(TA a)
		{
			 _version = unchecked(_version + 1);
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
	public class ReactiveVersion<TA, TB> : Vurbiri.Collections.Version, IUnsubscribed<Action<TA, TB>>
	{
		private Action<TA, TB> _action;

		[Impl(256)] public ReactiveVersion() => _action = Dummy.Action;

        [Impl(256)] public void Next(TA a, TB b)
		{
			 _version = unchecked(_version + 1);
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
	public class ReactiveVersion<TA, TB, TC> : Vurbiri.Collections.Version, IUnsubscribed<Action<TA, TB, TC>>
	{
		private Action<TA, TB, TC> _action;

		[Impl(256)] public ReactiveVersion() => _action = Dummy.Action;

        [Impl(256)] public void Next(TA a, TB b, TC c)
		{
			 _version = unchecked(_version + 1);
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
	public class ReactiveVersion<TA, TB, TC, TD> : Vurbiri.Collections.Version, IUnsubscribed<Action<TA, TB, TC, TD>>
	{
		private Action<TA, TB, TC, TD> _action;

		[Impl(256)] public ReactiveVersion() => _action = Dummy.Action;

		[Impl(256)] public void Next(TA a, TB b, TC c, TD d)
		{
			 _version = unchecked(_version + 1);
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
