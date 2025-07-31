using System;
using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    public class WaitParticle : InstantParticle
    {
        public WaitParticle(CreatorWaitParticle creator, Action<APooledSFX> deactivate) : base(creator, deactivate)
        {
        }

        public override IEnumerator Hit(ISFXUser user, ActorSkin target)
        {
            base.Hit(user, target);
            return this;
        }
    }
}
