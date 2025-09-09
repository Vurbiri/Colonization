using System;
using System.Runtime.CompilerServices;

namespace Vurbiri.Colonization.Actors
{
	public abstract class ACastParticle : AParticle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected ACastParticle(ParticleCreator creator, Action<APooledSFX> deactivate) : base(creator, deactivate) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected new void Setup(ActorSkin target)
        {
            base.Setup(target);
            target.Play(_clip);
        }
    }
}
