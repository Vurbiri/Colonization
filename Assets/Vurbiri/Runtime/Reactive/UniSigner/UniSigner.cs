//Assets\Vurbiri\Runtime\Reactive\UniSigner\UniSigner.cs
using System;
using UnityEngine;

namespace Vurbiri.Reactive
{
    [Serializable]
	public class UniSigner : Signer
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
    public class UniSigner<T> : Signer<T>
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
    public class UniSigner<TA, TB> : Signer<TA, TB>
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
    public class UniSigner<TA, TB, TC> : Signer<TA, TB, TC>
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
    public class UniSigner<TA, TB, TC, TD> : Signer<TA, TB, TC, TD>
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
