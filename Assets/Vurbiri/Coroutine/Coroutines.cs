using System.Collections;
using UnityEngine;

namespace Vurbiri
{
    public class Coroutines : MonoBehaviour
    {
        private const string NAME = @"[Coroutine for {0}]";
        
        public static Coroutines Create(string nameParent, bool isDontDestroy)
        {
            GameObject go = new(string.Format(NAME, nameParent));

            if (isDontDestroy)
                DontDestroyOnLoad(go);

            return go.GetComponent<Coroutines>();
        }

        public Coroutine Run(IEnumerator routine) => StartCoroutine(routine);

        public void Stop(Coroutine routine) => StopCoroutine(routine);

        public void Destroy() => Destroy(gameObject);
    }
}
