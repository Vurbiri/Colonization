using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class AParticle : APooledSFX, IEnumerator
    {
        protected readonly AudioClip _clip;
        protected readonly ParticleSystem _particle;

        public object Current => null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AParticle(AParticleCreatorSFX creator, Action<APooledSFX> deactivate) : base(creator, deactivate)
        {
            _clip = creator.Clip;
            _particle = creator.Particle;
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
