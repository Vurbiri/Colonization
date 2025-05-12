//Assets\Colonization\Scripts\UI\_UIGame\Panels\Currencies\BloodPanel.cs
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
    public class BloodPanel : MonoBehaviour
	{
        [SerializeField] private CurrentMaxPopup _blood;

        public void Init(Direction2 directionPopup, ACurrenciesReactive currencies, ProjectColors colors)
        {
            _blood.Init(currencies.BloodCurrent, currencies.BloodMax, colors, directionPopup);

            Destroy(this);
        }

#if UNITY_EDITOR
        public RectTransform UpdateVisuals_Editor(float pixelsPerUnit, Vector2 padding, ProjectColors colors)
        {
            Image image = GetComponent<Image>();
            image.color = colors.BackgroundPanel;
            image.pixelsPerUnitMultiplier = pixelsPerUnit;

            RectTransform thisRectTransform = (RectTransform)transform;
            thisRectTransform.sizeDelta = _blood.Size + padding * 2f;

            _blood.Init_Editor(colors);
            return thisRectTransform;
        }

        private void OnValidate()
        {
            if (_blood == null)
                _blood = GetComponentInChildren<CurrentMaxPopup>();
        }
#endif
    }
}
