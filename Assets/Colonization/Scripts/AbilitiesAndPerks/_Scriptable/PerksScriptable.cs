using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "Perks", menuName = "Vurbiri/Colonization/Perks", order = 51)]
    public class PerksScriptable : ScriptableObject
    {
        [SerializeField] private Perk[] _perks;

        public IReadOnlyList<IPerk> Perks => _perks;
        public IReadOnlyList<IPerkUI> PerksUI => _perks;

    }
}
