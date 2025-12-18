using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI;

    sealed public class MoveUI : ASkillUI
    {
        public MoveUI(SeparatorEffectUI separator) : base(separator, CONST.MOVE_SKILL_COST)
        {
            Localization.Subscribe(SetTexts);
        }

        private void SetTexts(Localization localization)
        {
            StringBuilder sb = new(SIZE);

            sb.AppendLine(localization.GetText(FILE, MOVE_KEY));
            _separator.GetText(sb);

            _textMain = sb.ToString();

            sb.Clear();
            sb.AppendLine(MOVE);
            sb.Append(localization.GetFormatText(FILE, AP_KEY, _cost));

            _textAP = sb.ToString();
        }

        public override void Dispose()
        {
            Localization.Unsubscribe(SetTexts);
        }
    }
}
