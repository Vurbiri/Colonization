//Assets\Colonization\Scripts\Actors\SFX\Hit\HitReactSoundSFX.cs
using UnityEngine;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class HitReactSoundSFX : IHitSFX
    {
        private readonly AudioClip _clip;
        private readonly AudioSource _audioSource;

        public HitReactSoundSFX(AudioClip clip, AudioSource audioSource)
        {
            _clip = clip;
            _audioSource = audioSource;
        }

        public void Hit(ActorSkin target)
        {
            _audioSource.PlayOneShot(_clip);
            target.React();
        }

    }
}
