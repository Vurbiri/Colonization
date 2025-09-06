using System;

namespace Vurbiri.Colonization.Actors
{
    sealed public class CreatorInstantParticle : ACreatorParticle
    {
        public override APooledSFX Create(Action<APooledSFX> deactivate) => new InstantParticle(this, deactivate);
    }
}
