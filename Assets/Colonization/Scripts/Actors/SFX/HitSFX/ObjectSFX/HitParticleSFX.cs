using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    sealed public class HitParticleSFX : AMonoSFX
    {
        [SerializeField] private AudioClip _clip;

        private ParticleSystem _particle;

        public override IHitSFX Init(Action<AMonoSFX> deactivate)
		{
            _particle = GetComponent<ParticleSystem>();

            return base.Init(deactivate); ;
        }

        public override IEnumerator Hit(IUserSFX user, ActorSkin target)
        {
            _thisTransform.SetParent(target.Transform, false);

            _particle.Play();
            user.AudioSource.PlayOneShot(_clip);

            return null;
        }

        private void OnParticleSystemStopped()
        {
            a_deactivate(this);
        }
	}
}
