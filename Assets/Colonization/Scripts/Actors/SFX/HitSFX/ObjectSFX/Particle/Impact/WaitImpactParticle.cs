using System;
using System.Collections;

namespace Vurbiri.Colonization
{
    sealed public class WaitImpactParticle : AImpactParticle
    {
        public WaitImpactParticle(AParticleCreatorSFX creator, Action<APooledSFX> deactivate) : base(creator, deactivate) { }

        public override IEnumerator Run(ActorSFX user, ActorSkin target)
        {
            Setup(target);
            return this;
        }
    }
}
