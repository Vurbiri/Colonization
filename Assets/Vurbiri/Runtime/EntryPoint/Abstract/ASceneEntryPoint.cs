using System;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.EntryPoint
{
    [DefaultExecutionOrder(-10)]
    public abstract class ASceneEntryPoint : MonoBehaviour
    {
        [SerializeField] protected ReactiveValue<SceneId> _defaultNextScene;

        private static ASceneEntryPoint _instance;

        public static event Action<ASceneEntryPoint> EventLoading;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                EventLoading?.Invoke(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public abstract IReactive<SceneId> Enter(SceneContainers containers);

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

    }
}
