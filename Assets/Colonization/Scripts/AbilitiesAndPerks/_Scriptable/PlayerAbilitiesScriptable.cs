using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "PlayerAbilities", menuName = "Vurbiri/Colonization/AbilitiesAndPerks/PlayerAbilities", order = 51)]
    public class PlayerAbilitiesScriptable : ScriptableObject
    {
        [SerializeField] private IdArray<PlayerAbilityId, int> _abilities;

        public int Count => _abilities.Count;

        public AbilitySet<PlayerAbilityId> GetAbilities() => new(_abilities);
    }
}
