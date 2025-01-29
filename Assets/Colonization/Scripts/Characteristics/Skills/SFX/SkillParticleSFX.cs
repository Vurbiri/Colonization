//Assets\Colonization\Scripts\Characteristics\Skills\SFX\SkillParticleSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    public class SkillParticleSFX : ASkillMonoSFX
    {
        [SerializeField] AudioClip _clip;

        private ParticleSystem _particle;
        private AudioSource _audioSource;

        public override ISkillSFX Init(IActorSFX parent, float duration)
		{
            _particle = GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = _particle.main;
            main.duration = duration;
            main.startLifetime = duration;

            _audioSource = parent.AudioSource;

            return this;
        }

        public override void Run(Transform target)
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
