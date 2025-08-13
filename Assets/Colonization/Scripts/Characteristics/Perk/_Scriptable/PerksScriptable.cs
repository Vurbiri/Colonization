using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Characteristics
{
	//[CreateAssetMenu(fileName = "Perks", menuName = "Vurbiri/Colonization/PerksScriptable", order = 51)]
	public class PerksScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private Perk[] _economicPerks;
        [SerializeField] private Perk[] _militaryPerks;
        private ReadOnlyArray<Perk>[] _readOnlyPerks;

        public ReadOnlyArray<Perk> this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                _readOnlyPerks ??= new ReadOnlyArray<Perk>[] { new(_economicPerks), new(_militaryPerks) };
                return _readOnlyPerks[index];
            }
        }
    }
}
