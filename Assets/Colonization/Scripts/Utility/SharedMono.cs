using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public class SharedMono : UnityEngine.MonoBehaviour
    {
        [SerializeField] private Transform _thisTransform;
        [SerializeField] private AudioSource _thisAudioSource;

        public Transform Transform { [Impl(256)] get => _thisTransform; }
        public AudioSource Audio { [Impl(256)] get => _thisAudioSource; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetComponent(ref _thisTransform);
            this.SetComponent(ref _thisAudioSource);
        }
#endif
    }
}
