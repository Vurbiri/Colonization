using System;
using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    sealed public class WaitHitParticle : AHitParticle
    {
        public WaitHitParticle(CreatorWaitHitParticle creator, Action<APooledSFX> deactivate) : base(creator, deactivate) { }

        public override IEnumerator Hit(ISFXUser user, ActorSkin target)
        {
            Setup(target);
            return this;
        }
    }
}
