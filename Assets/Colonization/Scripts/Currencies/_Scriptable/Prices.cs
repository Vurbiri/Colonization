using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
    //[CreateAssetMenu(fileName = "Prices", menuName = "Vurbiri/Colonization/Prices", order = 51)]
    public class Prices : ScriptableObject
    {
        [SerializeField] private CurrenciesLite _playersDefault;
        [SerializeField] private CurrenciesLite _roads;
        [SerializeField] private CurrenciesLite _wall;
        [SerializeField] private IdArray<EdificeId, CurrenciesLite> _edifices;
        [SerializeField] private IdArray<WarriorId, CurrenciesLite> _warriors;

        public CurrenciesLite HumanDefault => _playersDefault;
        public CurrenciesLite Road => _roads;
        public CurrenciesLite Wall => _wall;
        public ReadOnlyIdArray<EdificeId, CurrenciesLite> Edifices => _edifices;
        public ReadOnlyIdArray<WarriorId, CurrenciesLite> Warriors => _warriors;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _playersDefault ??= new();
            _roads ??= new();
            _wall ??= new();
            _edifices ??= new(() => new());
            _warriors ??= new(() => new());
        }
#endif
    }
}
