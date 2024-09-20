using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "PlayerAbilities", menuName = "Vurbiri/Colonization/PlayerAbilities", order = 51)]
    public class PlayerAbilitiesScriptable : ScriptableObject, IEnumerable<Ability>
    {
        [SerializeField] private List<Ability> _abilities;

        public int Count => _abilities.Count;

        public IEnumerator<Ability> GetEnumerator() => _abilities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
