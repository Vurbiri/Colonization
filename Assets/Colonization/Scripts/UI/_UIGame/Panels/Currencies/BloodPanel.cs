//Assets\Colonization\Scripts\UI\_UIGame\Panels\Currencies\BloodPanel.cs
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Collections;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class BloodPanel : MonoBehaviour
	{
        [SerializeField] private Blood _blood;

        public void Init(Color color, Direction2 directionPopup, ACurrenciesReactive currencies, TextColorSettings settings)
        {
            GetComponent<Image>().color = color;
            _blood.Init(currencies.BloodCurrent, currencies.BloodMax, settings, directionPopup);

            Destroy(this);
        }

#if UNITY_EDITOR
        public RectTransform UpdateVisuals_Editor(float pixelsPerUnit, Vector2 padding, IdArray<CurrencyId, CurrencyIcon> icons, TextColorSettings colors)
        {
            GetComponent<Image>().pixelsPerUnitMultiplier = pixelsPerUnit;
            RectTransform thisRectTransform = (RectTransform)transform;
            thisRectTransform.sizeDelta = _blood.Size + padding * 2f;
            _blood.Init_Editor(icons[CurrencyId.Blood], colors);
            return thisRectTransform;
        }

        private void OnValidate()
        {
            if (_blood == null)
                _blood = GetComponentInChildren<Blood>();
        }
#endif
    }
}
