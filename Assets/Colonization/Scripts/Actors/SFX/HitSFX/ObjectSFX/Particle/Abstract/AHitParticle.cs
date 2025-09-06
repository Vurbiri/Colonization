using System;
using System.Runtime.CompilerServices;

namespace Vurbiri.Colonization.Actors
{
	public abstract class AHitParticle : AParticle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected AHitParticle(ACreatorParticle creator, Action<APooledSFX> deactivate) : base(creator, deactivate) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected new void Setup(ActorSkin target)
        {
            base.Setup(target);
            target.Impact(_clip);
        }
    }
}
