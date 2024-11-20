using System.Text;
using Vurbiri.Localization;

namespace Vurbiri.Colonization.UI
{
    public class PermEffectUI : AEffectsUI
    {
        public PermEffectUI(string descKey, int value, string hexColor) : base(descKey, value, hexColor)
        {
        }

        public override void GetText(Language language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetTextFormat(Files.Gameplay, _descKey, _value));
        }
    }
}
