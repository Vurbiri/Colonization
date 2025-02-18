//Assets\Colonization\Scripts\UI\_UIGame\Characteristics\Effects\PenetrationEffectUI.cs
using System.Text;
using Vurbiri.Localization;

namespace Vurbiri.Colonization.UI
{
    public class PenetrationEffectUI : AEffectsUI
    {
        private readonly string _valueDefense;

        public PenetrationEffectUI(string descKey, string value, int valueDefense, string hexColor) : base(descKey, value, hexColor)
        {
            _valueDefense = (100 - valueDefense).ToString();
        }

        public override void GetText(Language language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetTextFormat(CONST_UI_LNG_KEYS.FILE, _descKey, _value, _valueDefense));
        }
    }
}
