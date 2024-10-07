using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    [CreateAssetMenu(fileName = "CurrenciesIcons", menuName = "Vurbiri/Colonization/CurrenciesIcons", order = 51)]
    public class CurrenciesIconsScriptable : ScriptableObject
    {
        [SerializeField] private IdArray<CurrencyId, CurrencyIcon> _icons;

        public CurrencyIcon this[int index] => _icons[index];
        public CurrencyIcon this[Id<CurrencyId> id] => _icons[id.ToInt];


#if UNITY_EDITOR
        public IdArray<CurrencyId, CurrencyIcon> Icons => _icons;
#endif
    }
}
