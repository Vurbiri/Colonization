using System;
using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    sealed public class ParticleOnTarget : AParticleOnTarget
    {
        public ParticleOnTarget(ParticleCreator creator, Action<APooledSFX> deactivate) : base(creator, deactivate) { }

        public override IEnumerator Run(ActorSFX user, ActorSkin target)
        {
            Setup(target);
            this.Start();
            yield break;
        }
    }
}
