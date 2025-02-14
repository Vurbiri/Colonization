//Assets\Colonization\Scripts\UI\_UIGame\Characteristics\Effects\ReflectEffectUI.cs
using System.Text;
using Vurbiri.Localization;

namespace Vurbiri.Colonization.UI
{
    public class ReflectEffectUI : AEffectsUI
    {
        private readonly string _descKeyReflect;
        private readonly string _valueReflect;
        private readonly string _hexColorReflect;

        public ReflectEffectUI(string descKey, string value, string hexColor, string descKeyReflect, int valueReflect, string hexColorReflect) 
                        : base(descKey, value, hexColor)
        {
            _descKeyReflect = descKeyReflect;
            _valueReflect = valueReflect.ToString();
            _hexColorReflect = hexColorReflect;
        }

        public override void GetText(Language language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetTextFormat(CONST_UI_LNG_KEYS.FILE, _descKey, _value));
            sb.Append(_hexColorReflect);
            sb.AppendLine(language.GetTextFormat(CONST_UI_LNG_KEYS.FILE, _descKeyReflect, _valueReflect));
        }
    }
}
