using System;
using UnityEngine;

namespace Vurbiri
{
    [Serializable]
	public class UVAction : VAction
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
                    _action -= action;
                    _action += action;
                }
            }

            _listeners = null;
        }
    }
    //=======================================================================================
    [Serializable]
    public class UVAction<T> : VAction<T>
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
                    _action -= action;
                    _action += action;
                }
            }

            _listeners = null;
        }
    }
    //=======================================================================================
    [Serializable]
    public class UVAction<TA, TB> : VAction<TA, TB>
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
                    _action += action;
                }
            }

            _listeners = null;
        }
    }
    //=======================================================================================
    [Serializable]
    public class UVAction<TA, TB, TC> : VAction<TA, TB, TC>
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
                    _action += action;
                }
            }

            _listeners = null;
        }
    }
    //=======================================================================================
    [Serializable]
    public class UVAction<TA, TB, TC, TD> : VAction<TA, TB, TC, TD>
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
                    _action += action;
                }
            }

            _listeners = null;
        }
    }
}
