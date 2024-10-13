using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "Prices", menuName = "Vurbiri/Colonization/Prices", order = 51)]
    public class PricesScriptable : ScriptableObject
    {
        [SerializeField] private CurrenciesLite _playersDefault;
        [SerializeField] private CurrenciesLite _roads;
        [SerializeField] private CurrenciesLite _wall;
        [SerializeField] private IdArray<EdificeId, CurrenciesLite> _edifices;

        public CurrenciesLite this[int index] { get => _edifices[index]; }
        public CurrenciesLite this[Id<EdificeId> id] { get => _edifices[id]; }

        public ACurrencies PlayersDefault => _playersDefault;
        public ACurrencies Road => _roads;
        public ACurrencies Wall => _wall;
        public IReadOnlyList<ACurrencies> Edifices => _edifices;
    }
}
