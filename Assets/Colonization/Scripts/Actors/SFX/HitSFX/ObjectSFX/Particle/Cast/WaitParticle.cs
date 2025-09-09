using System;
using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    sealed public class WaitParticle : ACastParticle
    {
        public WaitParticle(ParticleCreator creator, Action<APooledSFX> deactivate) : base(creator, deactivate) { }
        
        public override IEnumerator Hit(ActorSFX user, ActorSkin target)
        {
            Setup(target);
            return this;
        }
    }
}
