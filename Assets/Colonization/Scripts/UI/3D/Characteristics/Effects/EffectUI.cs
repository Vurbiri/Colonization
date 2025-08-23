using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    public class EffectUI : AEffectUI
    {
        protected readonly string _descKey;
        protected readonly string _hexColor;

        public EffectUI(string key, string hexColor)
        {
            _descKey = key;
            _hexColor = hexColor;
        }

        public override void GetText(Localization language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetText(CONST_UI_LNG_KEYS.FILE, _descKey));
        }
    }
}
