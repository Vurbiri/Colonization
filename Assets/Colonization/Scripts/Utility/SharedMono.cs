using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public class SharedMono : UnityEngine.MonoBehaviour
    {
        [SerializeField] private Transform _thisTransform;

        public Transform Transform { [Impl(256)] get => _thisTransform; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetComponent(ref _thisTransform);
        }
#endif
    }
}
