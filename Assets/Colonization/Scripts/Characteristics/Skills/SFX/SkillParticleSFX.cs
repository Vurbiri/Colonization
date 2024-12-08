//Assets\Colonization\Scripts\Characteristics\Skills\SFX\SkillParticleSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    public class SkillParticleSFX : ASkillMonoSFX
    {
        private ParticleSystem _particle;

        protected override ISkillSFX Init(float time)
		{
            _particle = GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = _particle.main;
            main.duration = time;
            main.startLifetime = time;

            return this;
        }

        public override void Run(Transform target)
        {
            _thisTransform.SetParent(target, false);
            _thisGO.SetActive(true);
            _particle.Play();
        }

        private void OnParticleSystemStopped()
        {
            _thisGO.SetActive(false);
            _thisTransform.SetParent(_parent, false);
        }
	}
}
