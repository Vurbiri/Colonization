//Assets\Colonization\Scripts\UI\_UIGame\Characteristics\SkillUI.cs
using System;
using System.Text;
using UnityEngine;
using Vurbiri.Localization;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI;
    using static CONST_UI_LNG_KEYS;

    [System.Serializable]
    public class SkillUI : IDisposable
    {
        [SerializeField] private string _nameKey;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private int _cost;

        private const int SIZE = 64;

        private AEffectsUI[] _effects;
        private string _textMain, _textAP;
        private string _hexColorPlus, _hexColorMinus;
        private int _capacity;

        private IUnsubscriber _unsubscriber;

        public Sprite Sprite => _sprite;
        public int Cost => _cost;

        public void Init(Language language, HintTextColor hintTextColor, AEffectsUI[] effects)
        {
            _hexColorPlus = hintTextColor.HexColorPlus;
            _hexColorMinus = hintTextColor.HexColorMinus;

            _effects = effects;

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
            int count = _effects.Length;
            StringBuilder sb = new(SIZE + count * SIZE);
            sb.AppendLine(localization.GetText(Files.Gameplay, _nameKey));
            for (int i = 0; i < count; i++)
                _effects[i].GetText(localization, sb);
            sb.Append(TAG_COLOR_OFF);

            _textMain = sb.ToString();
            _textAP = localization.GetTextFormat(Files.Gameplay, AP_KEY, _cost);

            _capacity = _textMain.Length + +_hexColorPlus.Length + _textAP.Length;
        }

        public void Dispose()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
