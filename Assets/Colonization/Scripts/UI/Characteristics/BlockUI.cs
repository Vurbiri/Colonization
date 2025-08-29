using System.Text;
using Vurbiri.International;
using static Vurbiri.Colonization.Characteristics.ReactiveEffectsFactory;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI_LNG_KEYS;
    using static TAG;

    sealed public class BlockUI : ASkillUI
    {
        private readonly string _value;

        public BlockUI(ProjectColors colors, SeparatorEffectUI separator, int cost, int value) : base(colors, separator, cost)
        {
            _value = $"+{value}";

            Localization.Instance.Subscribe(SetTexts);
        }

        private void SetTexts(Localization localization)
        {
            StringBuilder sb = new(SIZE << 1);

            sb.AppendLine(localization.GetText(FILE, BLOCK_KEY));
            _separator.GetText(sb);

            sb.Append(_hexColorPlus);
            sb.AppendLine(localization.GetFormatText(FILE, BLOCK_DESK_KEY, _value, BLOCK_DURATION));
            _separator.GetText(sb);

            sb.Append(COLOR_OFF);

            _textMain = sb.ToString();
            _textAP = localization.GetFormatText(FILE, AP_KEY, _cost);
        }


        public override void Dispose()
        {
            Localization.Instance.Unsubscribe(SetTexts);
        }
    }
}
