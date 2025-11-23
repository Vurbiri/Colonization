using UnityEngine;

namespace Vurbiri
{
    sealed internal class CoroutineInternal : MonoBehaviour
	{
        private static CoroutineInternal s_instance;

        internal static MonoBehaviour Instance
        {
            get
            {
                if (s_instance == null && Application.isPlaying)
                    new GameObject("[CoroutineInternal]").AddComponent<CoroutineInternal>();
                return s_instance;
            }
        }

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
    }
}
