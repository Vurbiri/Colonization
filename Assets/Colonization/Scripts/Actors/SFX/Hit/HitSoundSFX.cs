//Assets\Colonization\Scripts\Actors\SFX\Hit\HitSoundSFX.cs
using UnityEngine;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class HitSoundSFX : IHitSFX
    {
        private readonly AudioClip _clip;
        private readonly AudioSource _audioSource;

        public HitSoundSFX(AudioClip clip, AudioSource audioSource)
        {
            _clip = clip;
            _audioSource = audioSource;
        }

        public CustomYieldInstruction Hit(ActorSkin target)
        {
            _audioSource.PlayOneShot(_clip);
            return null;
        }

    }
}
