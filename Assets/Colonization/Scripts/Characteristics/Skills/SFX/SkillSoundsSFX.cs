//Assets\Colonization\Scripts\Characteristics\Skills\SFX\SkillSoundsSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    public class SkillSoundsSFX : ASkillSoundSFX
    {
        private readonly AudioClip[] _clips;

        public SkillSoundsSFX(AudioClip[] clips, AudioSource audioSource) : base(audioSource)
        {
            _clips = clips;
        }

        public override void Hint(int index) => _audioSource.PlayOneShot(_clips[index]);
    }
}
