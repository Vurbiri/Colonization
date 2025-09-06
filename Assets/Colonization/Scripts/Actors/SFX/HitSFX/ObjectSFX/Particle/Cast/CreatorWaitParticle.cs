using System;

namespace Vurbiri.Colonization.Actors
{
	sealed public class CreatorWaitParticle : ACreatorParticle
    {
        public override APooledSFX Create(Action<APooledSFX> deactivate) => new WaitParticle(this, deactivate);
    }
}
