//Assets\Colonization\Scripts\Characteristics\Abilities\_Scriptable\HumanAbilitiesScriptable.cs
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Characteristics
{
    //[CreateAssetMenu(fileName = "HumanAbilities", menuName = "Vurbiri/Colonization/Characteristics/HumanAbilities", order = 51)]
    sealed public class HumanAbilitiesScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private IdArray<HumanAbilityId, int> _abilities;

        public static implicit operator AbilitiesSet<HumanAbilityId>(HumanAbilitiesScriptable self) => new(self._abilities);
    }
}
