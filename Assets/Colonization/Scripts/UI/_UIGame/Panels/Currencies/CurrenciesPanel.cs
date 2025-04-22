//Assets\Colonization\Scripts\UI\_UIGame\Panels\Currencies\CurrenciesPanel.cs
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Collections;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class CurrenciesPanel : MonoBehaviour
    {
        [SerializeField] private Currency[] _currencies;
        [SerializeField] private Amount _amount;

        public void Init(Color color, Direction2 directionPopup, ACurrenciesReactive currencies, TextColorSettings settings)
        {
            GetComponent<Image>().color = color;

            for (int i = 0; i < CurrencyId.CountMain; i++)
                _currencies[i].Init(i, currencies, settings, directionPopup);

            _amount.Init(currencies.AmountCurrent, currencies.AmountMax, settings);

            Destroy(this);
        }

#if UNITY_EDITOR
        public RectTransform UpdateVisuals_Editor(float pixelsPerUnit, Vector2 padding, float space, IdArray<CurrencyId, CurrencyIcon> icons, TextColorSettings colors)
        {
            GetComponent<Image>().pixelsPerUnitMultiplier = pixelsPerUnit;

            RectTransform thisRectTransform = (RectTransform)transform;
            Vector2 cSize = _currencies[0].Size, aSize = _amount.Size;

            Vector2 size = new()
            {
                y = Mathf.Max(cSize.y, aSize.y) + padding.y * 2f,
                x = cSize.x * 5f + aSize.x + space * 5f + padding.x * 2f
            };
            thisRectTransform.sizeDelta = size;

            Vector2 pivot = thisRectTransform.pivot;
            float posX = cSize.x * 0.5f + padding.x - size.x * pivot.x;
            float posY = size.y * (0.5f - pivot.y);
            Vector3 pos = new(posX, posY, 0f);

            float offset = cSize.x + space;
            for (int i = 0; i < CurrencyId.CountMain; i++)
            {
                _currencies[i].Init_Editor(pos, icons[i], colors);
                pos.x += offset;
            }

            pos.x -= (cSize.x - aSize.x) * 0.5f;
            _amount.Init_Editor(pos, colors);

            return thisRectTransform;
        }


        private void OnValidate()
        {

            if (_currencies == null || _currencies.Length != CurrencyId.CountMain)
                _currencies = GetComponentsInChildren<Currency>();
            if (_amount == null)
                _amount = GetComponentInChildren<Amount>();
        }
#endif
    }
}
