//Assets\Vurbiri.EntryPoint\Runtime\Points\ASceneEntryPoint.cs
using System;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.EntryPoint
{
    public abstract class ASceneEntryPoint : MonoBehaviour
    {
        private static ASceneEntryPoint _instance;

        public static event Action<ASceneEntryPoint> EventLoaded;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                EventLoaded?.Invoke(this);
                return;
            }

            Destroy(gameObject);
        }

        public abstract ISigner<ExitParam> Enter(SceneContainer containers, Loading _loading, AEnterParam param);

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
    }
}
