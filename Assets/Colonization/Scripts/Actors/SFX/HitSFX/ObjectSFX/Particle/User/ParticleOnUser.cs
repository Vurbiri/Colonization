using System;
using System.Collections;

namespace Vurbiri.Colonization
{
    sealed public class ParticleOnUser : AParticleOnUser
    {
        public ParticleOnUser(AParticleCreatorSFX creator, Action<APooledSFX> deactivate) : base(creator, deactivate) { }

        public override IEnumerator Run(ActorSFX user, ActorSkin target)
        {
            Setup(user);
            this.Start();
            yield break;
        }
    }
}
