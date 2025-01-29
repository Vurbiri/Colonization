//Assets\Colonization\Scripts\Characteristics\Skills\SFX\Scriptables\SkillMonoScriptableSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    [CreateAssetMenu(fileName = "ScriptableSkillMonoSFX", menuName = "Vurbiri/Colonization/ActorSFX/SkillMonoScriptableSFX", order = 51)]
    public class SkillMonoScriptableSFX : AScriptableSFX
    {
        [SerializeField] private ASkillMonoSFX _prefab;

        public override ISkillSFX Create(IActorSFX parent, float duration)
        {
            if(_prefab == null)
                return new SkillEmptySFX();

            return Instantiate(_prefab, parent.Container).Init(parent, duration);
        }
    }
}
