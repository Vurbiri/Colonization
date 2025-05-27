using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class Amount : AHintWidget
    {
        private const string AMOUNT = "{0}{1,2}</color><space=0.05em>|<space=0.05em>{2,-2}";

        [SerializeField, Key(Files.Gameplay)] private string _keyOver;

        private Localization _localization;
        private ReactiveCombination<int, int> _reactiveAmountMax;
        private string _colorNormal, _colorNormalHint, _colorOver;

        public void Init(IReactiveValue<int> amount, IReactiveValue<int> max, ProjectColors colors, CanvasHint hint)
        {
            base.Init(colors, hint);

            _colorNormal = colors.PanelTextTag;
            _colorNormalHint = colors.HintDefaultTag;
            _colorOver = colors.TextNegativeTag;

            _localization = Localization.Instance;
            _reactiveAmountMax = new(amount, max, SetAmountMax);
        }

        private void SetAmountMax(int amount, int max)
        {
            if(amount > max)
            {
                _valueTMP.text = string.Format(AMOUNT, _colorOver, Mathf.Min(amount, 99), max);
                _text = _localization.GetFormatText(_getText.id, _getText.key, _colorOver, amount, max)
                                     .Concat(_localization.GetFormatText(_getText.id, _keyOver, amount - max));
            }
            else 
            {
                _valueTMP.text = string.Format(AMOUNT, _colorNormal, amount, max);
                _text = _localization.GetFormatText(_getText.id, _getText.key, _colorNormalHint, amount, max);
            }
        }

        private void OnDestroy()
        {
            _reactiveAmountMax.Dispose();
        }

#if UNITY_EDITOR
        public void Init_Editor(Vector3 position, ProjectColors settings)
        {
            Init_Editor(settings);
            transform.localPosition = position;
        }
#endif
    }
}
