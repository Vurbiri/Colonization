//Assets\Colonization\Scripts\Actors\SFX\Hit\HitParticleSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    public class HitParticleSFX : AHitMonoSFX
    {
        [SerializeField] AudioClip _clip;

        private ParticleSystem _particle;
        private AudioSource _audioSource;

        public override IHitSFX Init(IActorSFX parent)
		{
            _particle = GetComponent<ParticleSystem>();
            _audioSource = parent.AudioSource;

            return this;
        }

        public override void Hit(Transform target)
        {
            _thisTransform.SetParent(target, false);
            _thisGO.SetActive(true);
            _particle.Play();

            _audioSource.PlayOneShot(_clip);
        }

        private void OnParticleSystemStopped()
        {
            _thisGO.SetActive(false);
            _thisTransform.SetParent(_parent, false);
        }

	}
}
