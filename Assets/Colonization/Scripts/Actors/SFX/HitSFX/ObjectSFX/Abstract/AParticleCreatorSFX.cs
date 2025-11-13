using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public abstract class AParticleCreatorSFX : AMonoCreatorSFX
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private ParticleSystem _particle;
        [Space]
        [Tooltip("1 - Center")]
        [SerializeField, Range(0f, 2.5f)] private float _targetHeightRate;

        public AudioClip Clip { [Impl(256)] get => _clip; }
        public ParticleSystem Particle { [Impl(256)] get => _particle; }
        public float HeightRate { [Impl(256)] get => _targetHeightRate; }


#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetComponent(ref _particle);
        }
#endif
    }
}
