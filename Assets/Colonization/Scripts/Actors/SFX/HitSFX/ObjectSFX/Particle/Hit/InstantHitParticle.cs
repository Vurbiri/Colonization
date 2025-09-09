using System;
using System.Collections;

namespace Vurbiri.Colonization.Actors
{
	sealed public class InstantHitParticle : AHitParticle
    {
        public InstantHitParticle(ParticleCreator creator, Action<APooledSFX> deactivate) : base(creator, deactivate) { }

        public override IEnumerator Hit(ActorSFX user, ActorSkin target)
        {
            Setup(target);
            this.Start();
            return null;
        }
    }
}
