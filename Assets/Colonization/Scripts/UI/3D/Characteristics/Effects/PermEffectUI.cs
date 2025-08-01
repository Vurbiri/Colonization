using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    sealed public class PermEffectUI : AEffectsUI
    {
        public PermEffectUI(string descKey, string value, string hexColor) : base(descKey, value, hexColor)
        {
        }

        public override void GetText(Localization language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetFormatText(CONST_UI_LNG_KEYS.FILE, _descKey, _value));
        }
    }
}
