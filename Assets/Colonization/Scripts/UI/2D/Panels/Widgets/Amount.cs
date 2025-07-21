using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class Amount : AHintWidget
    {
        private const string AMOUNT = "{0}{1,2}</color><space=0.05em>|<space=0.05em>{2,-2}";

        [SerializeField, Key(LangFiles.Gameplay)] private string _keyOver;

        private ReactiveCombination<int, int> _reactiveAmountMax;
        private string _colorNormal, _colorNormalHint, _colorOver;
        private string _textNormalHint, _textOverHint;

        public void Init(IReactive<int> amount, IReactive<int> max, ProjectColors colors, CanvasHint hint)
        {
            base.Init(hint);

            _colorNormal = colors.PanelTextTag;
            _colorNormalHint = colors.HintDefaultTag;
            _colorOver = colors.TextNegativeTag;

            _reactiveAmountMax = new(amount, max, SetAmountMax);
        }

        private void SetAmountMax(int amount, int max)
        {
            if(amount > max)
            {
                _valueTMP.text = string.Format(AMOUNT, _colorOver, Mathf.Min(amount, 99), max);
                _text = string.Format(_textNormalHint, _colorOver, amount, max).Concat(string.Format(_textOverHint, amount - max));
            }
            else 
            {
                _valueTMP.text = string.Format(AMOUNT, _colorNormal, amount, max);
                _text = string.Format(_textNormalHint, _colorNormalHint, amount, max);
            }
        }

        protected override void SetLocalizationText(Localization localization)
        {
            _textNormalHint = localization.GetText(_getText.id, _getText.key);
            _textOverHint = localization.GetText(_getText.id, _keyOver);
            _reactiveAmountMax?.Signal();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
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
