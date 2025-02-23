//Assets\Colonization\Scripts\UI\_UIGame\Characteristics\Effects\PenetrationEffectUI.cs
using System.Text;
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization.UI
{
    public class PenetrationEffectUI : AEffectsUI
    {
        private readonly string _pierce;

        public PenetrationEffectUI(string descKey, string value, int pierce, string hexColor) : base(descKey, value, hexColor)
        {
            _pierce = pierce.ToString();
        }

        public override void GetText(Localization language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetTextFormat(CONST_UI_LNG_KEYS.FILE, _descKey, _value, _pierce));
        }
    }
}
