using System.Collections.ObjectModel;
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
	//[CreateAssetMenu(fileName = "Perks", menuName = "Vurbiri/Colonization/PerksScriptable", order = 51)]
	public class PerksScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private Perk[] _economicPerks;
        [SerializeField] private Perk[] _militaryPerks;
        private ReadOnlyCollection<Perk>[] _readOnlyPerks;

        public ReadOnlyCollection<Perk> this[int index]
        {
            get
            {
                _readOnlyPerks ??= new ReadOnlyCollection<Perk>[] { new(_economicPerks), new(_militaryPerks) };
                return _readOnlyPerks[index];
            }
        }
    }
}
