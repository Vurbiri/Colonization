using System;
using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    sealed public class WaitImpactParticle : AImpactParticle
    {
        public WaitImpactParticle(ParticleCreator creator, Action<APooledSFX> deactivate) : base(creator, deactivate) { }

        public override IEnumerator Run(ActorSFX user, ActorSkin target)
        {
            Setup(target);
            return this;
        }
    }
}
