using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    sealed public class TempEffectUI : AEffectsUI
    {
        private readonly string _duration;
        
        public TempEffectUI(string descKey, string value, int duration, string hexColor) : base(descKey, value, hexColor)
        {
            _duration = duration.ToString();
        }

        public override void GetText(Localization language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetFormatText(CONST_UI_LNG_KEYS.FILE, _descKey, _value, _duration));
        }
    }
}
