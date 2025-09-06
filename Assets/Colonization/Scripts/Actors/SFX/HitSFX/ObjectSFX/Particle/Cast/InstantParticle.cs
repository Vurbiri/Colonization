using System;
using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    sealed public class InstantParticle : ACastParticle
    {
        public InstantParticle(CreatorInstantParticle creator, Action<APooledSFX> deactivate) : base(creator, deactivate) { }

        public override IEnumerator Hit(ISFXUser user, ActorSkin target)
        {
            Setup(target);
            this.Start();
            return null;
        }
    }
}
