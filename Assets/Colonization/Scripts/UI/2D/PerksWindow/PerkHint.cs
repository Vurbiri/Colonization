using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
	sealed public class PerkHint : APerkHint
    {
        protected override void SetTextAndCost(Localization localization)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine(localization.GetText(Files.Gameplay, _key));
            stringBuilder.Append(_cost);

            _text = stringBuilder.ToString();
        }
        protected override void SetText(Localization localization)
        {
            _text = localization.GetText(Files.Gameplay, _key);
        }
    }
}
