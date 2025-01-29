//Assets\Colonization\Scripts\Characteristics\Skills\SFX\Abstract\ASkillSoundSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class ASkillSoundSFX : ISkillSFX
    {
        protected AudioSource _audioSource;

        public ASkillSoundSFX (AudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        public abstract void Hint(int index);

        public virtual void Run(Transform target) { }
    }
}
