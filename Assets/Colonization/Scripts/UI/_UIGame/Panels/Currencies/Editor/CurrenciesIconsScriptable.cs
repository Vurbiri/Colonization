//Assets\Colonization\Scripts\UI\_UIGame\Panels\Currencies\Editor\CurrenciesIconsScriptable.cs
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.UI
{
    //[CreateAssetMenu(fileName = "CurrenciesIcons", menuName = "Vurbiri/Colonization/CurrenciesIcons", order = 51)]
    public class CurrenciesIconsScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private IdArray<CurrencyId, CurrencyIcon> _icons;

        public IdArray<CurrencyId, CurrencyIcon> Icons => _icons;
    }
}
