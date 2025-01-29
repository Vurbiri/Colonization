//Assets\Colonization\Scripts\Characteristics\Skills\SFX\SkillSoundSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    public class SkillSoundSFX : ASkillSoundSFX
    {
        private readonly AudioClip _clip;

        public SkillSoundSFX(AudioClip clip, AudioSource audioSource) : base(audioSource)
        {
            _clip = clip;
        }

        public override void Hint(int index) => _audioSource.PlayOneShot(_clip);

    }
}
