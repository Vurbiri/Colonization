//Assets\Vurbiri\Runtime\Reactive\UniSigner\UniSigner.cs
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

        public void Init()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            if (_listeners == null) return;

            for (int i = _listeners.Length - 1; i >= 0; i--)
            {
                if (_listeners[i].TryCreateDelegate(out Action<T> action))
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
    public class UniSubscription<TA, TB> : Subscription<TA, TB>
    {
        [SerializeField] private Listener<TA, TB>[] _listeners;

        public void Init()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            if (_listeners == null) return;

            for (int i = _listeners.Length - 1; i >= 0; i--)
            {
                if (_listeners[i].TryCreateDelegate(out Action<TA, TB> action))
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
    public class UniSubscription<TA, TB, TC> : Subscription<TA, TB, TC>
    {
        [SerializeField] private Listener<TA, TB, TC>[] _listeners;

        public void Init()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            if (_listeners == null) return;

            for (int i = _listeners.Length - 1; i >= 0; i--)
            {
                if (_listeners[i].TryCreateDelegate(out Action<TA, TB, TC> action))
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
    public class UniSubscription<TA, TB, TC, TD> : Subscription<TA, TB, TC, TD>
    {
        [SerializeField] private Listener<TA, TB, TC, TD>[] _listeners;

        public void Init()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            if (_listeners == null) return;

            for (int i = _listeners.Length - 1; i >= 0; i--)
            {
                if (_listeners[i].TryCreateDelegate(out Action<TA, TB, TC, TD> action))
                {
                    actions -= action;
                    actions += action;
                }
            }

            _listeners = null;
        }
    }
}
