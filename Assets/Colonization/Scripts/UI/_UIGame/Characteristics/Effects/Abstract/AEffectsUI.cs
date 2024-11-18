using System.Text;
using Vurbiri.Localization;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI_LNG_KEYS;

    public abstract class AEffectsUI
	{
        protected readonly string _descKey;
        protected readonly int _value;
        protected readonly string _hexColor;

        public AEffectsUI(string key, int value, string hexColor)
        {
            _descKey = key;
            _value = value;
            _hexColor = hexColor;
        }

        public virtual void GetText(Language language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetTextFormat(FILE, _descKey, _value));
        }
    }
}
