using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class HitReactSoundSFX : IHitSFX
    {
        private readonly AudioClip _clip;

        public HitReactSoundSFX(AudioClip clip)
        {
            _clip = clip;
        }

        public IEnumerator Hit(ActorSkin target)
        {
            target.React(_clip);
            return null;
        }

    }
}
