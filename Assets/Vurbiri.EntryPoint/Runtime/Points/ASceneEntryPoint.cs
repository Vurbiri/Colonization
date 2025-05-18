//Assets\Vurbiri.EntryPoint\Runtime\Points\ASceneEntryPoint.cs
using System;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.EntryPoint
{
    public abstract class ASceneEntryPoint : MonoBehaviour
    {
        private static ASceneEntryPoint s_instance;

        public static event Action<ASceneEntryPoint> EventLoaded;

        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
                EventLoaded?.Invoke(this);
                return;
            }

            Destroy(gameObject);
        }

        public abstract ISigner<ExitParam> Enter(SceneContainer containers, Loading loading, AEnterParam param);

        protected virtual void OnDestroy()
        {
            if (s_instance == this)
                s_instance = null;
        }
    }
}
