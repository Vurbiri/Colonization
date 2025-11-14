using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri
{
    sealed public class Coroutines : MonoBehaviour
    {
        internal static Coroutines s_instance;

        private void Awake()
        {
            if (s_instance == null)
            {
                DontDestroyOnLoad(gameObject);
                s_instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static void Create()
        {
            if (s_instance == null)
            {
                GameObject obj = new("[Coroutine]");
                DontDestroyOnLoad(obj);
                s_instance = obj.AddComponent<Coroutines>();
            }
        }

        private void OnDestroy()
        {
            if(s_instance == this)
                s_instance = null;
        }
    }

    public static class CoroutineExt
    {
        static CoroutineExt() => Coroutines.Create();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Coroutine Start(this IEnumerator routine) => Coroutines.s_instance.StartCoroutine(routine);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Stop(this Coroutine routine) => Coroutines.s_instance.StopCoroutine(routine);
    }
}
