//Assets\Vurbiri\Runtime\Reactive\UniSigner\Abstract\AListener.cs
using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Vurbiri.Reactive
{
    [Serializable]
    public abstract class AListener<TDelegate> where TDelegate : Delegate
    {
        public static readonly BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
        //public static readonly BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        [SerializeField] private Object _target;
        [SerializeField] private string _methodName = string.Empty;

        public bool TryCreateDelegate(out TDelegate action)
        {
            action = null;
            if (_target == null | string.IsNullOrEmpty(_methodName))
                return false;

            Type delegateType = typeof(TDelegate);

            MethodInfo method = _target.GetType().GetMethod(_methodName, flags, null, delegateType.GetGenericArguments(), null);
            if (method == null)
                return false;

            action = Delegate.CreateDelegate(delegateType, method.IsStatic ? null : _target, method) as TDelegate;
            return action != null;
        }
    }
}
