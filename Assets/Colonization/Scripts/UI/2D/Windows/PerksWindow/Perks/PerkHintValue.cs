using Vurbiri.Colonization.Characteristics;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
	public class PerkHintValue : APerkHint
    {
		private int _value;

        public override void Init(Perk perk)
        {
            _value = perk.Value;
            base.Init(perk);
        }

        protected override void SetTextAndCost(Localization localization)
        {
            _hintText = string.Concat(localization.GetFormatText(LangFiles.Gameplay, _key, _value), _cost);
        }
        protected override void SetText(Localization localization)
        {
            _hintText = localization.GetFormatText(LangFiles.Gameplay, _key, _value);
        }
    }
}
