//Assets\Colonization\Scripts\Actors\SFX\Hit\HitReactSoundSFX.cs
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

        public CustomYieldInstruction Hit(ActorSkin target)
        {
            target.React(_clip);
            return null;
        }

    }
}
