using System;
using System.Collections;

namespace Vurbiri.Colonization.Actors
{
	sealed public class InstantHitParticle : AHitParticle
    {
        public InstantHitParticle(CreatorInstantHitParticle creator, Action<APooledSFX> deactivate) : base(creator, deactivate) { }

        public override IEnumerator Hit(ISFXUser user, ActorSkin target)
        {
            Setup(target);
            this.Start();
            return null;
        }
    }
}
