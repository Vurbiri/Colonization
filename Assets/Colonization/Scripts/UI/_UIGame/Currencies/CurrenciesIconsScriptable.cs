using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    [CreateAssetMenu(fileName = "CurrenciesIcons", menuName = "Vurbiri/Colonization/CurrenciesIcons", order = 51)]
    public class CurrenciesIconsScriptable : ScriptableObject
    {
        [SerializeField] private EnumArray<CurrencyType, CurrencyIcon> _icons;
        [Space]
        [SerializeField] private CurrencyIcon _blood;

        public CurrencyIcon this[int index] => _icons[index];
        public CurrencyIcon this[CurrencyType type] => _icons[(int) type];

        public CurrencyIcon Blood => _blood;


#if UNITY_EDITOR
        public EnumArray<CurrencyType, CurrencyIcon> Icons => _icons;
#endif
    }
}
