//Assets\Colonization\Scripts\UI\_UIGame\Characteristics\Effects\TempEffectUI.cs
using System.Text;
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization.UI
{
    public class TempEffectUI : AEffectsUI
    {
        private readonly string _duration;
        
        public TempEffectUI(string descKey, string value, int duration, string hexColor) : base(descKey, value, hexColor)
        {
            _duration = duration.ToString();
        }

        public override void GetText(Localization language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetTextFormat(CONST_UI_LNG_KEYS.FILE, _descKey, _value, _duration));
        }
    }
}
