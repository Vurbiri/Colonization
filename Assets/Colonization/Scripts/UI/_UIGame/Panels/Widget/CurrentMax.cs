//Assets\Colonization\Scripts\UI\_UIGame\Panels\Widget\CurrentMax.cs
using TMPro;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class CurrentMax : AHintWidget
    {
        public const string COUNT = "{0,2}<space=0.05em>|<space=0.05em>{1,-2}";

        [Space]
        [SerializeField] private TextMeshProUGUI _countTMP;

        private Localization _localization;
        private ReactiveCombination<int, int> _reactiveCurrentMax;

        public void Init(IReactiveValue<int> current, IReactiveValue<int> max, ProjectColors colors, CanvasHint hint)
        {
            base.Init(hint);
            _countTMP.color = colors.PanelText;

            _localization = Localization.Instance;
            _reactiveCurrentMax = new(current, max, SetCurrentMax);
        }

        private void SetCurrentMax(int current, int max)
        {
            _countTMP.text = string.Format(COUNT, current, max);
            _text = _localization.GetFormatText(_getText.id, _getText.key, current, max);
        }

        private void OnDestroy()
        {
            _reactiveCurrentMax.Dispose();
        }

#if UNITY_EDITOR
        public Vector2 Size => ((RectTransform)transform).rect.size;
        public void Init_Editor(ProjectColors settings)
        {
            _countTMP.color = settings.PanelText;
        }

        protected virtual void OnValidate()
        {
            if (_countTMP == null)
                _countTMP = EUtility.GetComponentInChildren<TextMeshProUGUI>(this, "TextTMP");
        }
#endif
    }
}
