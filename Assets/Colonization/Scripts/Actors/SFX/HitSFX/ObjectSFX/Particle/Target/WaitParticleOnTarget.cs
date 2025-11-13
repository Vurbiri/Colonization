using System;
using System.Collections;

namespace Vurbiri.Colonization
{
    sealed public class WaitParticleOnTarget : AParticleOnTarget
    {
        public WaitParticleOnTarget(AParticleCreatorSFX creator, Action<APooledSFX> deactivate) : base(creator, deactivate) { }
        
        public override IEnumerator Run(ActorSFX user, ActorSkin target)
        {
            Setup(target);
            return this;
        }
    }
}
