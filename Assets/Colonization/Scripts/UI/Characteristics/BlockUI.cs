using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI;
    using static TAG;

    sealed public class BlockUI : ASkillUI
    {
        private readonly string _value;

        public BlockUI(SeparatorEffectUI separator, int cost, int value) : base(separator, cost)
        {
            _value = $"+{value}";

            Localization.Subscribe(SetTexts);
        }

        private void SetTexts(Localization localization)
        {
            StringBuilder sb = new(SIZE << 1);

            sb.AppendLine(localization.GetText(FILE, BLOCK_KEY));
            _separator.GetText(sb);

            sb.Append(GameContainer.UI.Colors.TextPositiveTag);
            sb.AppendLine(localization.GetFormatText(FILE, BLOCK_DESK_KEY, _value, CONST.BLOCK_DURATION));
            _separator.GetText(sb);

            sb.Append(COLOR_OFF);

            _textMain = sb.ToString();
            _textAP = localization.GetFormatText(FILE, AP_KEY, _cost);
        }

        public override void Dispose()
        {
            Localization.Unsubscribe(SetTexts);
        }
    }
}
