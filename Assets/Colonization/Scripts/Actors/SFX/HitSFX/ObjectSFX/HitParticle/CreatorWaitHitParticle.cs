using System;

namespace Vurbiri.Colonization.Actors
{
	public class CreatorWaitHitParticle : CreatorInstantHitParticle
    {
        public override APooledSFX Create(Action<APooledSFX> deactivate) => new WaitHitParticle(this, deactivate);
    }
}
