using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public abstract class AParticleOnUser : APooledSFX, IEnumerator
    {
        private readonly AudioClip _clip;
        private readonly ParticleSystem _particle;

        public object Current => null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected AParticleOnUser(ParticleCreator creator, Action<APooledSFX> deactivate) : base(creator, deactivate)
        {
            _clip = creator.Clip;
            _particle = creator.Particle;
        }

        public bool MoveNext()
        {
            if (_particle.isPlaying)
                return true;

            _transform.SetParent(GameContainer.SharedContainer, false);
            Disable();
            return false;
        }

        public void Reset() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Setup(ActorSFX user)
        {
            _transform.SetParent(user.TargetTransform, false);
            _gameObject.SetActive(true);

            _particle.Play();
            user.Play(_clip);
        }
    }
}
