using System;
using System.Collections;

namespace Vurbiri.Colonization
{
    sealed public class WaitParticleOnUser : AParticleOnUser
    {
        public WaitParticleOnUser(AParticleCreatorSFX creator, Action<APooledSFX> deactivate) : base(creator, deactivate) { }

        public override IEnumerator Run(ActorSFX user, ActorSkin target)
        {
            Setup(user);
            return this;
        }
    }
}
