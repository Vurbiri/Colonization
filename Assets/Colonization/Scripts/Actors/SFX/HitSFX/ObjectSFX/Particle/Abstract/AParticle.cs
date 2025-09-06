using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public abstract class AParticle : APooledSFX, IEnumerator
    {
        protected readonly AudioClip _clip;
        private readonly ParticleSystem _particle;
        private readonly float _targetHeightRate;

        public object Current => null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AParticle(ACreatorParticle creator, Action<APooledSFX> deactivate) : base(creator, deactivate)
        {
            _clip = creator.clip;
            _particle = creator.particle;
            _targetHeightRate = creator.targetHeightRate;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Setup(ActorSkin target)
        {
            Vector3 targetPosition = target.Transform.position;
            targetPosition.y += target.Bounds.extents.y * _targetHeightRate;

            Enable(targetPosition);

            _particle.Play();
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
