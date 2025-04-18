//Assets\Colonization\Scripts\UI\_UIGame\Currencies\Blood.cs
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class Blood : MonoBehaviour
    {
        private const string COUNT = "{0}<space=0.1em>({1})";

        [SerializeField] private TMP_Text _countTMP;
        [SerializeField] private PopupWidgetUI _popup;
        [Space]
        [SerializeField] private RectTransform _thisRectTransform;

        private ReactiveCombination<int, int> _reactiveBlood;

        public Vector2 Size => _thisRectTransform.sizeDelta;

        public void Init(Vector3 position, IReactiveValue<int> current, IReactiveValue<int> max, TextColorSettings settings, Direction2 offsetPopup)
        {
            _popup.Init(settings, offsetPopup);
            _thisRectTransform.localPosition = position;

            _countTMP.color = settings.ColorTextBase;

            _reactiveBlood = new(current, max);
            _reactiveBlood.Subscribe(SetBlood);
        }

        private void SetBlood(int current, int max)
        {
            _countTMP.text = string.Format(COUNT, current, max);
        }

        private void OnDestroy()
        {
            _reactiveBlood.Dispose();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_countTMP == null)
                _countTMP = GetComponent<TMP_Text>();
            if (_thisRectTransform == null)
                _thisRectTransform = GetComponent<RectTransform>();
            if (_popup == null)
                _popup = GetComponentInChildren<PopupWidgetUI>();

            EUtility.FindAnyScriptable<CurrenciesIconsScriptable>().Icons[CurrencyId.Blood].ToImage(GetComponentInChildren<Image>());
        }
#endif
    }
}
