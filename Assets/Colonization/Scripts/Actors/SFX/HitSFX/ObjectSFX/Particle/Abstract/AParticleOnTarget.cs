using System;
using System.Runtime.CompilerServices;

namespace Vurbiri.Colonization
{
	public abstract class AParticleOnTarget : AParticle
    {
        private readonly float _targetHeightRate;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected AParticleOnTarget(AParticleCreatorSFX creator, Action<APooledSFX> deactivate) : base(creator, deactivate) => _targetHeightRate = creator.HeightRate;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Setup(ActorSkin target)
        {
            Enable(target.GetPosition(_targetHeightRate));
            _particle.Play();
            target.Play(_clip);
        }
    }
}
