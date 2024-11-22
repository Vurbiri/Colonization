//Assets\Colonization\Editor\Currencies\Scripts\CurrenciesIconsScriptable.cs
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CreateAssetMenu(fileName = "CurrenciesIcons", menuName = "Vurbiri/Colonization/CurrenciesIcons", order = 51)]
    public class CurrenciesIconsScriptable : ScriptableObject
    {
        [SerializeField] private IdArray<CurrencyId, CurrencyIcon> _icons;

        public IdArray<CurrencyId, CurrencyIcon> Icons => _icons;

    }
}
