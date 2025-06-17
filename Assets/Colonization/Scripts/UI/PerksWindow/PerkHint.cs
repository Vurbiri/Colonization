using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
	sealed public class PerkHint : APerkHint
    {
        protected override void SetLocalizationText(Localization localization)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine(localization.GetText(Files.Gameplay, _key));
            stringBuilder.AppendLine();
            stringBuilder.Append(_cost);

            _text = stringBuilder.ToString();
        }
    }
}
