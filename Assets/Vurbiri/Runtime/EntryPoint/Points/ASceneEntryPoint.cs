//Assets\Vurbiri\Runtime\EntryPoint\Points\ASceneEntryPoint.cs
using System;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.EntryPoint
{
    [DefaultExecutionOrder(-10)]
    public abstract class ASceneEntryPoint : MonoBehaviour
    {
        

        private static ASceneEntryPoint _instance;

        public static event Action<ASceneEntryPoint> EventLoading;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                EventLoading?.Invoke(this);
                return;
            }

            Destroy(gameObject);
        }

        public abstract IReactive<ExitParam> Enter(SceneContainers containers, AEnterParam param);

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

    }
}
