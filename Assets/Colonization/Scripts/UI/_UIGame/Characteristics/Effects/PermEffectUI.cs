//Assets\Colonization\Scripts\UI\_UIGame\Characteristics\Effects\PermEffectUI.cs
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
            sb.AppendLine(language.GetTextFormat(CONST_UI_LNG_KEYS.FILE, _descKey, _value));
        }
    }
}
