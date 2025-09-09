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
        public AParticle(ParticleCreator creator, Action<APooledSFX> deactivate) : base(creator, deactivate)
        {
            _clip = creator.Clip;
            _particle = creator.Particle;
            _targetHeightRate = creator.HeightRate;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Setup(ActorSkin target)
        {
            Enable(target.GetPosition(_targetHeightRate));
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
