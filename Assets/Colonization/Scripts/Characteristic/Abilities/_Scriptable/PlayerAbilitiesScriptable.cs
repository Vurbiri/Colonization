using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "PlayerAbilities", menuName = "Vurbiri/Colonization/Characteristics/PlayerAbilities", order = 51)]
    public class PlayerAbilitiesScriptable : ScriptableObject
    {
        [SerializeField] private IdArray<PlayerAbilityId, int> _abilities;

        public AbilitiesSet<PlayerAbilityId> GetAbilities() => new(_abilities);
    }
}
