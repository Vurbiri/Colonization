//Assets\Vurbiri\Runtime\Reactive\UnitySubscriber\Abstract\AListener.cs
using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Vurbiri.Reactive
{
    [Serializable]
    public abstract class AListener<TDelegate> where TDelegate : Delegate
    {
        public static readonly BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        [SerializeField] private Object _target;
        [SerializeField] private string _methodName = string.Empty;

        private readonly Type actionType = typeof(TDelegate);

        public bool TryInstantiate(out TDelegate action)
        {
            action = null;
            if (_target is null | string.IsNullOrEmpty(_methodName))
                return false;

            MethodInfo method = _target.GetType().GetMethod(_methodName, flags, null, actionType.GetGenericArguments(), null);
            if (method == null)
                return false;

            action = Delegate.CreateDelegate(actionType, method.IsStatic ? null : _target, method) as TDelegate;
            return action != null;
        }
    }
}
