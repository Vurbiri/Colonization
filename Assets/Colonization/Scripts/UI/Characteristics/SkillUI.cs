using System;
using System.Text;
using UnityEngine;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI_LNG_KEYS;

    public class SkillUI : SkillUISettings, IDisposable
    {
        private const int SIZE = 78;

        private readonly AEffectUI[] _effectsTarget;
        private readonly AEffectUI[] _effectsSelf;
        private readonly SeparatorEffectUI _separator;
        private readonly string _hexColor, _hexColorPlus, _hexColorMinus;
        private string _textMain, _textAP;

        public string Key => _keyName;
        public Sprite Sprite => _sprite;
        public int Cost => _cost;

        public SkillUI(SkillUISettings settings, ProjectColors colors, AEffectUI[] effectsTarget, AEffectUI[] effectsSelf, SeparatorEffectUI separator) : base(settings)
        {
            _hexColor = colors.HintDefaultTag;
            _hexColorPlus = colors.TextPositiveTag;
            _hexColorMinus = colors.TextNegativeTag;

            _effectsTarget = effectsTarget;
            _effectsSelf = effectsSelf;
            _separator = separator;

            Localization.Instance.Subscribe(SetTexts);
        }

        public string GetText(bool isUse) => string.Concat(_textMain, isUse ? _hexColorPlus : _hexColorMinus, _textAP);

        private void SetTexts(Localization localization)
        {
            int countTarget = _effectsTarget.Length, countSelf = _effectsSelf.Length;

            StringBuilder sb = new(SIZE + countTarget * SIZE + countSelf * SIZE);
            sb.AppendLine(localization.GetText(FILE, _keyName));
            _separator.GetText(sb);

            if (countTarget > 0)
            {
                if (countSelf > 0)
                {
                    sb.Append(_hexColor);
                    sb.AppendLine(localization.GetText(FILE, ON_TARGET));
                }

                for (int i = 0; i < countTarget; i++)
                   _effectsTarget[i].GetText(localization, sb);
            }

            if (countSelf > 0)
            {
                sb.Append(_hexColor);
                sb.AppendLine(localization.GetText(FILE, ON_SELF));

                for (int i = 0; i < countSelf; i++)
                    _effectsSelf[i].GetText(localization, sb);
            }

            _textMain = sb.ToString();
            _textAP = localization.GetFormatText(FILE, AP_KEY, _cost);
        }

        public void Dispose()
        {
            Localization.Instance.Unsubscribe(SetTexts);
        }
    }
}
