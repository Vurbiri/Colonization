//Assets\Colonization\Scripts\UI\_UIGame\Panels\Currencies\Blood.cs
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class Blood : MonoBehaviour
    {
        private const string COUNT = "{0}<space=0.14em>({1})";

        [SerializeField] private TMP_Text _countTMP;
        [SerializeField] private PopupWidgetUI _popup;

        private ReactiveCombination<int, int> _reactiveBlood;

        public void Init(IReactiveValue<int> current, IReactiveValue<int> max, TextColorSettings settings, Direction2 offsetPopup)
        {
            _popup.Init(settings, offsetPopup);

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
        public Vector2 Size => ((RectTransform)transform).sizeDelta;

        public void Init_Editor(CurrencyIcon icon, TextColorSettings settings)
        {
            _countTMP.color = settings.ColorTextBase;
            SetBlood(12, 13);

            UnityEditor.SerializedObject serializedImage = new(GetComponentInChildren<Image>());
            serializedImage.Update();
            serializedImage.FindProperty("m_Color").colorValue = icon.Color;
            serializedImage.FindProperty("m_Sprite").objectReferenceValue = icon.Icon;
            serializedImage.ApplyModifiedProperties();
        }

        private void OnValidate()
        {
            if (_countTMP == null)
                _countTMP = GetComponent<TMP_Text>();
            if (_popup == null)
                _popup = GetComponentInChildren<PopupWidgetUI>();
        }
#endif
    }
}
