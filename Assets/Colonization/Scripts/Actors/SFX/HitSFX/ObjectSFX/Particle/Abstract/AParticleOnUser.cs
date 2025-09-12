using System;
using System.Runtime.CompilerServices;

namespace Vurbiri.Colonization.Actors
{
	public abstract class AParticleOnUser : AParticle
	{
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected AParticleOnUser(ParticleCreator creator, Action<APooledSFX> deactivate) : base(creator, deactivate) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Setup(ActorSFX user)
        {
            Enable(user.Position);
            _particle.Play();
            user.Play(_clip);
        }
    }
}
