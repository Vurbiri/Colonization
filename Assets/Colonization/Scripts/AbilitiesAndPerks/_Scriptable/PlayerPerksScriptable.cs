using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "PlayerPerks", menuName = "Vurbiri/Colonization/AbilitiesAndPerks/PlayerPerks", order = 51)]
    public class PlayerPerksScriptable : ScriptableObject
    {
        [SerializeField] private PlayerPerk[] _perks;

        public IReadOnlyList<IPerk<PlayerAbilityId>> Perks => _perks;
        public IReadOnlyList<IPerkUI> PerksUI => _perks;

    }
}
