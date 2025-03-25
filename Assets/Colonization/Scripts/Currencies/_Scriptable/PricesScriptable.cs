//Assets\Colonization\Scripts\Currencies\_Scriptable\PricesScriptable.cs
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
    //[CreateAssetMenu(fileName = "Prices", menuName = "Vurbiri/Colonization/Prices", order = 51)]
    public class PricesScriptable : ScriptableObject
    {
        [SerializeField] private CurrenciesLite _playersDefault;
        [SerializeField] private CurrenciesLite _roads;
        [SerializeField] private CurrenciesLite _wall;
        [SerializeField] private IdArray<EdificeId, CurrenciesLite> _edifices;
        [SerializeField] private IdArray<WarriorId, CurrenciesLite> _warriors;

        public ACurrencies HumanDefault => _playersDefault;
        public ACurrencies Road => _roads;
        public ACurrencies Wall => _wall;
        public IReadOnlyList<ACurrencies> Edifices => _edifices;
        public IReadOnlyList<ACurrencies> Warriors => _warriors;
    }
}
