using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri
{
    public class Coroutines : MonoBehaviour
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
                GameObject obj = new("[Coroutines]");
                DontDestroyOnLoad(obj);
                s_instance = obj.AddComponent<Coroutines>();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Coroutine Run(IEnumerator routine) => s_instance.StartCoroutine(routine);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Stop(Coroutine coroutine) => s_instance.StopCoroutine(coroutine);

        private void OnDestroy()
        {
            if(s_instance == this)
                s_instance = null;
        }
    }

    public static class CoroutineExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Coroutine Run(this IEnumerator self) => Coroutines.s_instance.StartCoroutine(self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Stop(this Coroutine self) => Coroutines.s_instance.StopCoroutine(self);
    }
}
