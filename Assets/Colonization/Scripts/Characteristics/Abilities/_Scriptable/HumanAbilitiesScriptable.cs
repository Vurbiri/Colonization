//Assets\Colonization\Scripts\Characteristics\Abilities\_Scriptable\HumanAbilitiesScriptable.cs
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    //[CreateAssetMenu(fileName = "HumanAbilities", menuName = "Vurbiri/Colonization/Characteristics/HumanAbilities", order = 51)]
    sealed public class HumanAbilitiesScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private IdArray<HumanAbilityId, int> _abilities;

        public AbilitiesSet<HumanAbilityId> Get(IReactive<Perk> perks) => new(_abilities, perks);
    }
}
