using System;

namespace Vurbiri.Colonization.Actors
{
	sealed public class CreatorWaitHitParticle : ACreatorParticle
    {
        
        public override APooledSFX Create(Action<APooledSFX> deactivate) => new WaitHitParticle(this, deactivate);
    }
}
