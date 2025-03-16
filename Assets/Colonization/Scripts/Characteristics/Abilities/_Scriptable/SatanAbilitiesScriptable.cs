//Assets\Colonization\Scripts\Characteristics\Abilities\_Scriptable\SatanAbilitiesScriptable.cs
using UnityEngine;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization
{
    //[CreateAssetMenu(fileName = "SatanAbilities", menuName = "Vurbiri/Colonization/Characteristics/SatanAbilities", order = 51)]
    public class SatanAbilitiesScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private SatanAbilities _abilities;

        public static implicit operator SatanAbilities(SatanAbilitiesScriptable self) => self._abilities;
    }
}
