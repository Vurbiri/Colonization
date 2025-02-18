//Assets\Colonization\Scripts\UI\_UIGame\Characteristics\Effects\ReflectPenetrationEffectUI.cs
using System.Text;
using Vurbiri.Localization;

namespace Vurbiri.Colonization.UI
{
    public class ReflectPenetrationEffectUI : ReflectEffectUI
    {
        private readonly string _valueDefense;

        public ReflectPenetrationEffectUI(string descKey, string value, int valueDefense, string hexColor, string descKeyReflect, int valueReflect, string hexColorReflect) : base(descKey, value, hexColor, descKeyReflect, valueReflect, hexColorReflect)
        {
            _valueDefense = (100 - valueDefense).ToString();
        }

        public override void GetText(Language language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetTextFormat(CONST_UI_LNG_KEYS.FILE, _descKey, _value, _valueDefense));
            sb.Append(_hexColorReflect);
            sb.AppendLine(language.GetTextFormat(CONST_UI_LNG_KEYS.FILE, _descKeyReflect, _valueReflect));
        }
    }
}
