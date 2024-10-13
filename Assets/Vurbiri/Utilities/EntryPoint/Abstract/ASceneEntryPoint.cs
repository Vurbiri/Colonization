using System;
using UnityEngine;

namespace Vurbiri
{
    [DefaultExecutionOrder(-10)]
    public abstract class ASceneEntryPoint : MonoBehaviour
    {
        private static ASceneEntryPoint _instance;

        public static event Action<ASceneEntryPoint> EventLoad;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                EventLoad?.Invoke(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public abstract void Enter(SceneContainers containers);

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

    }
}
