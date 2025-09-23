using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.EntryPoint
{
    public abstract class ASceneEntryPoint : MonoBehaviour
    {
        private static ASceneEntryPoint s_instance;
        private static readonly Subscription<ASceneEntryPoint> s_sceneLoaded = new();

        public static Event<ASceneEntryPoint> EventLoaded => s_sceneLoaded;

        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
                s_sceneLoaded.Invoke(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public abstract void Enter(Loading loading, Transition transition);

        private void OnDestroy()
        {
            if (s_instance == this)
                s_instance = null;
        }
    }
}
