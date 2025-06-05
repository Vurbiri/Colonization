using System.Collections.ObjectModel;
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    public class APlayerPerksScriptable<T> : ScriptableObjectDisposable where T : APerkId<T>
    {
        [SerializeField] private Perk[] _perks;
        private ReadOnlyCollection<Perk> _readOnlyPerks;

        public ReadOnlyCollection<Perk> Perks => _readOnlyPerks ??= new(_perks);

    }
}
