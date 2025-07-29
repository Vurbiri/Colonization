using System;
using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
    sealed public class InstantHitParticle : APooledSFX, IEnumerator
    {
        private readonly AudioClip _clip;
        private readonly ParticleSystem _particle;

        public object Current => null;

        public InstantHitParticle(CreatorInstantHitParticle creator, Action<APooledSFX> deactivate) : base(creator, deactivate)
        {
            _clip = creator.clip;
            _particle = creator.particle;
        }

        public override IEnumerator Hit(IUserSFX user, ActorSkin target)
        {
            Enable(target.Transform.position);

            _particle.Play();
            user.AudioSource.PlayOneShot(_clip);

            this.Start();

            return null;
        }

        public bool MoveNext()
        {
            if (_particle.isPlaying)
                return true;

            Disable();
            return false;
        }

        public void Reset() { }
    }
}
