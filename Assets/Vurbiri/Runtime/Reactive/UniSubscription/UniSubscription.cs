using System;
using UnityEngine;

namespace Vurbiri.Reactive
{
    [Serializable]
	public class UniSubscription : Subscription
	{
        [SerializeField] private Listener[] _listeners;

        public void Init()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            if (_listeners == null) return;

            for (int i = _listeners.Length - 1; i >= 0; i--)
            {
                if (_listeners[i].TryCreateDelegate(out Action action))
                {
                    actions -= action;
                    actions += action;
                }
            }

            _listeners = null;
        }
    }
    //=======================================================================================
    [Serializable]
    public class UniSubscription<T> : Subscription<T>
    {
        [SerializeField] private Listener<T>[] _listeners;

        public void Init(T value)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            if (_listeners == null) return;

            for (int i = _listeners.Length - 1; i >= 0; i--)
            {
                if (_listeners[i].TryCreateDelegate(out Action<T> action))
                {
                    action(value);
                    actions += action;
                }
            }

            _listeners = null;
        }
    }
    //=======================================================================================
    [Serializable]
    public class UniSubscription<TA, TB> : Subscription<TA, TB>
    {
        [SerializeField] private Listener<TA, TB>[] _listeners;

        public void Init(TA valueA, TB valueB)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            if (_listeners == null) return;

            for (int i = _listeners.Length - 1; i >= 0; i--)
            {
                if (_listeners[i].TryCreateDelegate(out Action<TA, TB> action))
                {
                    action(valueA, valueB);
                    actions += action;
                }
            }

            _listeners = null;
        }
    }
    //=======================================================================================
    [Serializable]
    public class UniSubscription<TA, TB, TC> : Subscription<TA, TB, TC>
    {
        [SerializeField] private Listener<TA, TB, TC>[] _listeners;

        public void Init(TA valueA, TB valueB, TC valueC)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            if (_listeners == null) return;

            for (int i = _listeners.Length - 1; i >= 0; i--)
            {
                if (_listeners[i].TryCreateDelegate(out Action<TA, TB, TC> action))
                {
                    action(valueA, valueB, valueC);
                    actions += action;
                }
            }

            _listeners = null;
        }
    }
    //=======================================================================================
    [Serializable]
    public class UniSubscription<TA, TB, TC, TD> : Subscription<TA, TB, TC, TD>
    {
        [SerializeField] private Listener<TA, TB, TC, TD>[] _listeners;

        public void Init(TA valueA, TB valueB, TC valueC, TD valueD)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            if (_listeners == null) return;

            for (int i = _listeners.Length - 1; i >= 0; i--)
            {
                if (_listeners[i].TryCreateDelegate(out Action<TA, TB, TC, TD> action))
                {
                    action(valueA, valueB, valueC, valueD);
                    actions += action;
                }
            }

            _listeners = null;
        }
    }
}
