using System;
using UnityEngine;

namespace Vurbiri
{
    public class Coroutines : MonoBehaviour, IDisposable
    {
        private const string NAME = @"[{0}]";

        private bool _isQuit = false;

        public static Coroutines Create(string name, bool isDontDestroy = false)
        {
            GameObject gObj = new(string.Format(NAME, name));

            if (isDontDestroy)
                DontDestroyOnLoad(gObj);

            return gObj.AddComponent<Coroutines>();
        }

        public void Dispose()
        {
            if (!_isQuit)
            {
                StopAllCoroutines();
                Destroy(gameObject);
            }
        }

        private void OnApplicationQuit() => _isQuit = true;
    }
}
