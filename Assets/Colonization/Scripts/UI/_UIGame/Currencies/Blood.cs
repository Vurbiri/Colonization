//Assets\Colonization\Scripts\UI\_UIGame\Currencies\Blood.cs
using TMPro;
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI;

    public class Blood : MonoBehaviour
    {
        private const string COUNT = "{0}({1})";

        [SerializeField] private TMP_Text _textTMP;
        [SerializeField] private PopupWidgetUI _popup;
        [Space]
        [SerializeField] private RectTransform _thisRectTransform;

        private ReactiveCombination<int, int> _reactiveBlood;
        private string _blood;

        public Vector2 Size => _thisRectTransform.sizeDelta;

        public void Init(Vector3 position, IReactive<int> current, IReactive<int> max, TextColorSettings settings, Direction2 offsetPopup)
        {
            _popup.Init(settings, offsetPopup);
            _thisRectTransform.localPosition = position;

            _blood = string.Format(TAG_SPRITE, CurrencyId.Blood).Concat(COUNT);
            _textTMP.color = settings.ColorTextBase;

            _reactiveBlood = new(current, max);
            _reactiveBlood.Subscribe(SetBlood);
        }

        private void SetBlood(int current, int max)
        {
            _textTMP.text = string.Format(_blood, current, max);
        }

        private void OnDestroy()
        {
            _reactiveBlood.Dispose();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_textTMP == null)
                _textTMP = GetComponent<TMP_Text>();
            if (_thisRectTransform == null)
                _thisRectTransform = GetComponent<RectTransform>();
            if (_popup == null)
                _popup = GetComponentInChildren<PopupWidgetUI>();
        }
#endif
    }
}
