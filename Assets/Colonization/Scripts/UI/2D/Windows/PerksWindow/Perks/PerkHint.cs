using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
	sealed public class PerkHint : APerkHint
    {
        protected override void SetTextAndCost(Localization localization)
        {
            _hintText = string.Concat(localization.GetText(LangFiles.Abilities, _key), _cost);
        }
        protected override void SetText(Localization localization)
        {
            _hintText = localization.GetText(LangFiles.Abilities, _key);
        }
    }
}
