using System;
using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    sealed public class ParticleOnUser : AParticleOnUser
    {
        public ParticleOnUser(ParticleCreator creator, Action<APooledSFX> deactivate) : base(creator, deactivate) { }

        public override IEnumerator Run(ActorSFX user, ActorSkin target)
        {
            Setup(user);
            this.Start();
            yield break;
        }
    }
}
