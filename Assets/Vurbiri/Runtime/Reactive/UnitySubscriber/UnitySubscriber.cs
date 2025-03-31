//Assets\Vurbiri\Runtime\Reactive\UnitySubscriber\UnitySubscriber.cs
using System;
using UnityEngine;

namespace Vurbiri.Reactive
{
    [Serializable]
	public class UnitySubscriber :  Subscriber, ISerializationCallbackReceiver
	{
        [SerializeField] private Listener[] _listeners;

        public void Clear()
        {
            if(Application.isPlaying) 
                _listeners = null;
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            if(_listeners == null) return;
            
            for (int i = _listeners.Length - 1; i >= 0; i--)
            {
                if (_listeners[i].TryInstantiate(out Action action))
                {
                    actions -= action;
                    actions += action;
                }
            }
        }
	}
    //=======================================================================================
    [Serializable]
    public class UnitySubscriber<T> : Subscriber<T>, ISerializationCallbackReceiver
    {
        [SerializeField] private Listener<T>[] _listeners;

        public void Clear()
        {
            if (Application.isPlaying)
                _listeners = null;
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            if (_listeners == null) return;

            for (int i = _listeners.Length - 1; i >= 0; i--)
            {
                if (_listeners[i].TryInstantiate(out Action<T> action))
                {
                    actions -= action;
                    actions += action;
                }
            }
        }
    }
    //=======================================================================================
    [Serializable]
    public class UnitySubscriber<TA, TB> : Subscriber<TA, TB>, ISerializationCallbackReceiver
    {
        [SerializeField] private Listener<TA, TB>[] _listeners;

        public void Clear()
        {
            if (Application.isPlaying)
                _listeners = null;
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            if (_listeners == null) return;

            for (int i = _listeners.Length - 1; i >= 0; i--)
            {
                if (_listeners[i].TryInstantiate(out Action<TA, TB> action))
                {
                    actions -= action;
                    actions += action;
                }
            }
        }
    }
    //=======================================================================================
    [Serializable]
    public class UnitySubscriber<TA, TB, TC> : Subscriber<TA, TB, TC>, ISerializationCallbackReceiver
    {
        [SerializeField] private Listener<TA, TB, TC>[] _listeners;

        public void Clear()
        {
            if (Application.isPlaying)
                _listeners = null;
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            if (_listeners == null) return;

            for (int i = _listeners.Length - 1; i >= 0; i--)
            {
                if (_listeners[i].TryInstantiate(out Action<TA, TB, TC> action))
                {
                    actions -= action;
                    actions += action;
                }
            }
        }
    }
    //=======================================================================================
    [Serializable]
    public class UnitySubscriber<TA, TB, TC, TD> : Subscriber<TA, TB, TC, TD>, ISerializationCallbackReceiver
    {
        [SerializeField] private Listener<TA, TB, TC, TD>[] _listeners;

        public void Clear()
        {
            if (Application.isPlaying)
                _listeners = null;
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            if (_listeners == null) return;

            for (int i = _listeners.Length - 1; i >= 0; i--)
            {
                if (_listeners[i].TryInstantiate(out Action<TA, TB, TC, TD> action))
                {
                    actions -= action;
                    actions += action;
                }
            }
        }
    }
}
