namespace Vurbiri.Colonization
{
    using Actors;
    using Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Prices", menuName = "Vurbiri/Colonization/Prices", order = 51)]
    public class PricesScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private CurrenciesLite _playersDefault;
        [SerializeField] private CurrenciesLite _roads;
        [SerializeField] private CurrenciesLite _wall;
        [SerializeField] private IdArray<EdificeId, CurrenciesLite> _edifices;
        [SerializeField] private IdArray<WarriorId, CurrenciesLite> _warriors;

        public ACurrencies PlayersDefault => _playersDefault;
        public ACurrencies Road => _roads;
        public ACurrencies Wall => _wall;
        public IReadOnlyList<ACurrencies> Edifices => _edifices;
        public IReadOnlyList<ACurrencies> Warriors => _warriors;
    }
}
