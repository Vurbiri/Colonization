using System;

namespace Vurbiri.Colonization.Actors
{
	public class CreatorWaitParticle : CreatorInstantParticle
    {
        public override APooledSFX Create(Action<APooledSFX> deactivate) => new WaitParticle(this, deactivate);
    }
}
