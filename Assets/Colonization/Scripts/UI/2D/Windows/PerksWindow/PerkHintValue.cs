using System.Text;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public class PerkHintValue : APerkHint
    {
		private int _value;

        public override void Init(Perk perk, CanvasHint hint)
        {
            _value = perk.Value;
            base.Init(perk, hint);
        }

        protected override void SetTextAndCost(Localization localization)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendFormat(localization.GetText(LangFiles.Gameplay, _key), _value);
            stringBuilder.AppendLine();
            stringBuilder.Append(_cost);

            _text = stringBuilder.ToString(); ;
        }
        protected override void SetText(Localization localization)
        {
            _text = localization.GetFormatText(LangFiles.Gameplay, _key, _value);
        }
    }
}
