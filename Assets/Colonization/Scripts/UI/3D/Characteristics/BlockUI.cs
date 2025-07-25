using System;
using System.Text;
using Vurbiri.International;
using Vurbiri.Reactive;
using static Vurbiri.Colonization.Characteristics.ReactiveEffectsFactory;

namespace Vurbiri.Colonization.UI
{
    using static TAG;
    using static CONST_UI_LNG_KEYS;

    public class BlockUI : IDisposable
    {
        private const int SIZE = 64;

        private readonly Unsubscription _unsubscriber;
        private readonly int _cost;
        private readonly string _value;
        private readonly string _hexColorPlus, _hexColorMinus;
        private string _textMain, _textAP;
        private int _capacity;

        public int Cost => _cost;

        public BlockUI(int cost, int value)
        {
            _cost = cost;
            _value = value > 0 ? $"+{value}" : value.ToString();

            _hexColorPlus  = GameContainer.UI.Colors.TextPositiveTag;
            _hexColorMinus = GameContainer.UI.Colors.TextNegativeTag;

            _unsubscriber = Localization.Instance.Subscribe(SetTexts);
        }

        public string GetText(bool isUse)
        {
            StringBuilder sb = new(_textMain, _capacity);
            sb.Append(isUse ? _hexColorPlus : _hexColorMinus);
            sb.Append(_textAP);

            return sb.ToString();
        }

        private void SetTexts(Localization localization)
        {
            StringBuilder sb = new(SIZE << 1);
            sb.AppendLine(localization.GetText(FILE, BLOCK_KEY));
            sb.Append(_hexColorPlus);
            sb.AppendLine(localization.GetFormatText(FILE, BLOCK_DESK_KEY, _value, BLOCK_DURATION));
            sb.Append(COLOR_OFF);

            _textMain = sb.ToString();
            _textAP = localization.GetFormatText(FILE, AP_KEY, _cost);

            _capacity = _textMain.Length + +_hexColorPlus.Length + _textAP.Length;
        }

        public void Dispose()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
