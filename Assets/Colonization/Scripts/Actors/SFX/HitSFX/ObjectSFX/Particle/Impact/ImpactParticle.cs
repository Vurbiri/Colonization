using System;
using System.Collections;

namespace Vurbiri.Colonization.Actors
{
	sealed public class ImpactParticle : AImpactParticle
    {
        public ImpactParticle(ParticleCreator creator, Action<APooledSFX> deactivate) : base(creator, deactivate) { }

        public override IEnumerator Run(ActorSFX user, ActorSkin target)
        {
            Setup(target);
            this.Start();
            yield break;
        }
    }
}
