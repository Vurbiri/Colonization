using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class InstantParticle : APooledSFX, IEnumerator
    {
        private readonly AudioClip _clip;
        private readonly ParticleSystem _particle;
        private readonly float _targetHeightRate;

        public object Current => null;

        public InstantParticle(CreatorInstantParticle creator, Action<APooledSFX> deactivate) : base(creator, deactivate)
        {
            _clip = creator.clip;
            _particle = creator.particle;
            _targetHeightRate = creator.targetHeightRate;
        }

        public override IEnumerator Hit(ISFXUser user, ActorSkin target)
        {
            Vector3 targetPosition = target.Transform.position;
            targetPosition.y += target.Bounds.extents.y * _targetHeightRate;

            Enable(targetPosition);

            target.ActorSFX.Impact(_clip);
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
