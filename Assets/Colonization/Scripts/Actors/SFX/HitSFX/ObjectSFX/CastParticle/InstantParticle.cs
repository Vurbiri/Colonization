using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class InstantParticle : APooledSFX, IEnumerator
    {
        private readonly AudioClip _clip;
        private readonly ParticleSystem _particle;

        public object Current => null;

        public InstantParticle(CreatorInstantParticle creator, Action<APooledSFX> deactivate) : base(creator, deactivate)
        {
            _clip = creator.clip;
            _particle = creator.particle;
        }

        public override IEnumerator Hit(ISFXUser user, ActorSkin target)
        {
            Enable(target.Transform.position);

            target.ActorSFX.AudioSource.PlayOneShot(_clip);
            _particle.Play();

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
