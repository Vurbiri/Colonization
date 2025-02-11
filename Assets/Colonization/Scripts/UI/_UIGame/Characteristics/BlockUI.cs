//Assets\Colonization\Scripts\UI\_UIGame\Characteristics\BlockUI.cs
using System;
using System.Text;
using Vurbiri.Localization;
using Vurbiri.Reactive;
using Vurbiri.UI;
using static Vurbiri.Colonization.Characteristics.EffectsFactory;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI;
    using static CONST_UI_LNG_KEYS;

    public class BlockUI : IDisposable
    {
        private const int SIZE = 64;

        private readonly IUnsubscriber _unsubscriber;
        private readonly int _cost, _value;
        private readonly string _hexColorPlus, _hexColorMinus;
        private string _textMain, _textAP;
        private int _capacity;

        public int Cost => _cost;

        public BlockUI(int cost, int value)
        {
            _cost = cost;
            _value = value;

            var hintTextColor = SceneData.Get<SettingsTextColor>();
            _hexColorPlus = hintTextColor.HexColorPositive;
            _hexColorMinus = hintTextColor.HexColorNegative;

            _unsubscriber = SceneServices.Get<Language>().Subscribe(SetTexts);
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
            StringBuilder sb = new(SIZE << 1);
            sb.AppendLine(localization.GetText(FILE, BLOCK_KEY));
            sb.Append(_hexColorPlus);
            sb.AppendLine(localization.GetTextFormat(FILE, BLOCK_DESK_KEY, _value, BLOCK_DURATION));
            sb.Append(TAG_COLOR_OFF);

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
