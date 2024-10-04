using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "PlayerAbilities", menuName = "Vurbiri/Colonization/PlayerAbilities", order = 51)]
    public class PlayerAbilitiesScriptable : ScriptableObject
    {
        // [SerializeField] private EnumArray<PlayerAbilityType, int> _abilities;
        [SerializeField] private List<int> _abilities;

        public int Count => _abilities.Count;

        public IdHashSet<IdPlayerAbility, Ability> GetAbilities()
        {
            IdHashSet<IdPlayerAbility, Ability> set = new();
            for(int i = 0; i < IdPlayerAbility.Count; i++)
                set.Add(new(i, _abilities[i]));

            return set;
        }
    }
}
