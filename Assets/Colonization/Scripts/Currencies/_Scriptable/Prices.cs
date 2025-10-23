using UnityEngine;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public class Prices : ScriptableObject
    {
        [SerializeField] private CurrenciesLite _playersDefault;
        [SerializeField] private ReadOnlyMainCurrencies _roads;
        [SerializeField] private ReadOnlyMainCurrencies _wall;
        [SerializeField] private ReadOnlyIdArray<EdificeId, ReadOnlyMainCurrencies> _edifices;
        [SerializeField] private ReadOnlyIdArray<WarriorId, ReadOnlyMainCurrencies> _warriors;

        public CurrenciesLite HumanDefault { [Impl(256)] get => _playersDefault; }
        public ReadOnlyMainCurrencies Road { [Impl(256)] get => _roads; }
        public ReadOnlyMainCurrencies Wall { [Impl(256)] get => _wall; }
        public ReadOnlyIdArray<EdificeId, ReadOnlyMainCurrencies> Edifices { [Impl(256)] get => _edifices; }
        public ReadOnlyIdArray<WarriorId, ReadOnlyMainCurrencies> Warriors { [Impl(256)] get => _warriors; }

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
