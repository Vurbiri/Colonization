using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "PlayerAbilities", menuName = "Vurbiri/Colonization/PlayerAbilities", order = 51)]
    public class PlayerAbilitiesScriptable : ScriptableObject
    {
        [SerializeField] private EnumArray<PlayerAbilityType, int> _abilities;

        public int Count => _abilities.Count;

        public EnumHashSet<PlayerAbilityType, Ability> GetAbilities()
        {
            EnumHashSet<PlayerAbilityType, Ability> set = new();
            PlayerAbilityType[] types = Enum<PlayerAbilityType>.Values;

            foreach (PlayerAbilityType type in types)
                set.Add(new(type, _abilities[type]));

            return set;
        }
    }
}
