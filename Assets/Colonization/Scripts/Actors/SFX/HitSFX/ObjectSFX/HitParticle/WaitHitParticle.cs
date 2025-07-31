using System;
using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    public class WaitHitParticle : InstantHitParticle
    {
        public WaitHitParticle(CreatorWaitHitParticle creator, Action<APooledSFX> deactivate) : base(creator, deactivate)
        {
        }

        public override IEnumerator Hit(ISFXUser user, ActorSkin target)
        {
            base.Hit(user, target);
            return this;
        }
    }
}
