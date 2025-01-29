//Assets\Colonization\Scripts\Characteristics\Skills\SFX\Abstract\AScriptableSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    //[CreateAssetMenu(fileName = "AScriptableSFX", menuName = "Vurbiri/Colonization/ActorSFX/AScriptableSFX", order = 51)]
    public abstract class AScriptableSFX : ScriptableObject
	{
        public abstract ISkillSFX Create(IActorSFX parent, float duration);
    }
}
