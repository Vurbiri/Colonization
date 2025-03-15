//Assets\Colonization\Scripts\Actors\SFX\Hit\HitParticleSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    sealed public class HitParticleSFX : AHitMonoSFX
    {
        [SerializeField] private AudioClip _clip;

        private ParticleSystem _particle;
        private AudioSource _audioSource;

        public override IHitSFX Init(IDataSFX parent)
		{
            Init();
            
            _particle = GetComponent<ParticleSystem>();
            _audioSource = parent.AudioSource;

            return this;
        }

        public override CustomYieldInstruction Hit(ActorSkin target)
        {
            _thisTransform.SetParent(target.Transform, false);
            _thisGO.SetActive(true);
            _particle.Play();

            _audioSource.PlayOneShot(_clip);

            return null;
        }

        private void OnParticleSystemStopped()
        {
            _thisGO.SetActive(false);
            _thisTransform.SetParent(_parent, false);
        }
	}
}
