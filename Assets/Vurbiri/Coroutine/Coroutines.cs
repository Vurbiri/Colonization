using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri
{
    public class Coroutines : MonoBehaviour, IDisposable
    {
        private const string NAME = @"[{0}]";
        
        public static Coroutines Create(string name, bool isDontDestroy)
        {
            GameObject go = new(string.Format(NAME, name));

            if (isDontDestroy)
                DontDestroyOnLoad(go);

            return go.AddComponent<Coroutines>();
        }

        public Coroutine Run(IEnumerator routine) => StartCoroutine(routine);

        public void Stop(Coroutine routine) => StopCoroutine(routine);

        public void Dispose() => Destroy(gameObject);
    }
}
