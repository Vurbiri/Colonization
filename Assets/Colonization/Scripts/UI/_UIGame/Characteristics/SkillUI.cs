//Assets\Colonization\Scripts\UI\_UIGame\Characteristics\SkillUI.cs
using System;
using System.Text;
using UnityEngine;
using Vurbiri.Localization;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI_LNG_KEYS;

    [System.Serializable]
    public class SkillUI : IDisposable
    {
        [SerializeField] private int _idNameKey;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private int _cost;

        private const int SIZE = 64;

        private AEffectsUI[] _effectsTarget;
        private AEffectsUI[] _effectsSelf;
        private int _countTarget, _countSelf;
        private string _textMain, _textAP;
        private string _hexColor, _hexColorPlus, _hexColorMinus;
        private int _capacity;

        private IUnsubscriber _unsubscriber;

        public Sprite Sprite => _sprite;
        public int Cost => _cost;

        public void Init(Language language, SettingsTextColor hintTextColor, AEffectsUI[] effectsTarget, AEffectsUI[] effectsSelf)
        {
            _hexColor = hintTextColor.HexColorHintBase;
            _hexColorPlus = hintTextColor.HexColorPositive;
            _hexColorMinus = hintTextColor.HexColorNegative;

            _effectsTarget = effectsTarget;
            _countTarget = effectsTarget.Length;

            _effectsSelf = effectsSelf;
            _countSelf = effectsSelf.Length;

            _unsubscriber = language.Subscribe(SetTexts);
        }

        public string GetText(bool isUse)
        {
            StringBuilder sb = new(_textMain, _capacity);
            sb.Append(isUse ? _hexColorPlus : _hexColorMinus);
            sb.Append(_textAP);

            return sb.ToString();
        }

        private void SetTexts(Language localization)
        {
            StringBuilder sb = new(SIZE + _countTarget * SIZE + _countSelf * SIZE);
            sb.AppendLine(localization.GetText(FILE, KEYS_NAME_SKILLS[_idNameKey]));
            if (_countTarget > 0)
            {
                if (_countSelf > 0)
                    sb.AppendLine(localization.GetText(FILE, ON_TARGET));

                for (int i = 0; i < _countTarget; i++)
                    _effectsTarget[i].GetText(localization, sb);
            }
            if (_countSelf > 0)
            {
                if (_countTarget > 0)
                {
                    sb.Append(_hexColor);
                    sb.AppendLine(localization.GetText(FILE, ON_SELF));
                }

                for (int i = 0; i < _countSelf; i++)
                    _effectsSelf[i].GetText(localization, sb);
            }

            _textMain = sb.ToString();
            _textAP = localization.GetTextFormat(FILE, AP_KEY, _cost);

            _capacity = _textMain.Length + +_hexColorPlus.Length + _textAP.Length;
        }

        public void Dispose()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
