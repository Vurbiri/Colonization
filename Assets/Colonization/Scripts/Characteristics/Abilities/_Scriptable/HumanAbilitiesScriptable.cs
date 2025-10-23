using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    //[CreateAssetMenu(fileName = "HumanAbilities", menuName = "Vurbiri/Colonization/Characteristics/HumanAbilities", order = 51)]
    sealed public class HumanAbilitiesScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private IdArray<HumanAbilityId, int> _abilities;

        public AbilitiesSet<HumanAbilityId> Get(PerkTree perks) => new(_abilities, perks);
    }
}
