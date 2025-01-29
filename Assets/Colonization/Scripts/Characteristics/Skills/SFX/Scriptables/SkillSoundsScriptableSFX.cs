//Assets\Colonization\Scripts\Characteristics\Skills\SFX\Scriptables\SkillSoundsScriptableSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    [CreateAssetMenu(fileName = "ScriptableSkillSoundSFX", menuName = "Vurbiri/Colonization/ActorSFX/SkillSoundsScriptableSFX", order = 51)]
	public class SkillSoundsScriptableSFX : AScriptableSFX
    {
        [SerializeField] private AudioClip[] _clips;

        public override ISkillSFX Create(IActorSFX parent, float duration)
        {
            if(_clips == null)
                return new SkillEmptySFX();

            int count = _clips.Length;

            if(count == 0)
                return new SkillEmptySFX();

            if(count == 1)
                return new SkillSoundSFX(_clips[0], parent.AudioSource);

            return new SkillSoundsSFX(_clips, parent.AudioSource);
        }
    }
}
