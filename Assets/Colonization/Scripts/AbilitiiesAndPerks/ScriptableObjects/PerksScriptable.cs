using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "Perks", menuName = "Vurbiri/Colonization/Perks", order = 51)]
    public class PerksScriptable : ScriptableObject
    {
        [SerializeField] private Perk[] _perks;

        
    }
}
