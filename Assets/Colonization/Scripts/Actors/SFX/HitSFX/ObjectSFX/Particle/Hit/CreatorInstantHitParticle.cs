using System;

namespace Vurbiri.Colonization.Actors
{
	sealed public class CreatorInstantHitParticle : ACreatorParticle
    {
        
        public override APooledSFX Create(Action<APooledSFX> deactivate) => new InstantHitParticle(this, deactivate);
    }
}
