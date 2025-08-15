using System;
using System.Text;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI_LNG_KEYS;

    [System.Serializable]
    public class SkillUI : IDisposable
    {
        [SerializeField] private string _keyName;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private int _cost;

        private const int SIZE = 64;

        private AEffectsUI[] _effectsTarget;
        private AEffectsUI[] _effectsSelf;
        private string _textMain, _textAP;
        private string _hexColor, _hexColorPlus, _hexColorMinus;

        private Unsubscription _unsubscriber;

        public Sprite Sprite => _sprite;
        public int Cost => _cost;

        public void Init(ProjectColors colors, AEffectsUI[] effectsTarget, AEffectsUI[] effectsSelf)
        {
            _hexColor = colors.HintDefaultTag;
            _hexColorPlus = colors.TextPositiveTag;
            _hexColorMinus = colors.TextNegativeTag;

            _effectsTarget = effectsTarget;
            _effectsSelf = effectsSelf;

            _unsubscriber = Localization.Instance.Subscribe(SetTexts);
        }

        public string GetText(bool isUse) => string.Concat(_textMain, isUse ? _hexColorPlus : _hexColorMinus, _textAP);

        private void SetTexts(Localization localization)
        {
            int countTarget = _effectsTarget.Length, countSelf = _effectsSelf.Length;

            StringBuilder sb = new(SIZE + countTarget * SIZE + countSelf * SIZE);
            sb.AppendLine(localization.GetText(FILE, _keyName));

            if (countTarget > 0)
            {
                if (countSelf > 0)
                    sb.AppendLine(localization.GetText(FILE, ON_TARGET));

                for (int i = 0; i < countTarget; i++)
                    _effectsTarget[i].GetText(localization, sb);
            }

            if (countSelf > 0)
            {
                if (countTarget > 0)
                    sb.AppendLine();

                sb.Append(_hexColor);
                sb.AppendLine(localization.GetText(FILE, ON_SELF));

                for (int i = 0; i < countSelf; i++)
                    _effectsSelf[i].GetText(localization, sb);
            }

            sb.AppendLine();

            _textMain = sb.ToString();
            _textAP = localization.GetFormatText(FILE, AP_KEY, _cost);
        }

        public void Dispose()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
