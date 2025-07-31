using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class ImpactSound : IHitSFX
    {
        private readonly AudioClip _clip;

        public ImpactSound(AudioClip clip)
        {
            _clip = clip;
        }

        public IEnumerator Hit(ISFXUser user, ActorSkin target)
        {
            target.Impact(_clip);
            return null;
        }

    }
}
