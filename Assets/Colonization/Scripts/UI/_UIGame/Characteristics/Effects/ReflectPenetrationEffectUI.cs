using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    sealed public class ReflectPenetrationEffectUI : ReflectEffectUI
    {
        private readonly string _pierce;

        public ReflectPenetrationEffectUI(string descKey, string value, int pierce, string hexColor, string descKeyReflect, int valueReflect, string hexColorReflect) : base(descKey, value, hexColor, descKeyReflect, valueReflect, hexColorReflect)
        {
            _pierce = pierce.ToString();
        }

        public override void GetText(Localization language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetFormatText(CONST_UI_LNG_KEYS.FILE, _descKey, _value, _pierce));
            sb.Append(_hexColorReflect);
            sb.AppendLine(language.GetFormatText(CONST_UI_LNG_KEYS.FILE, _descKeyReflect, _valueReflect));
        }
    }
}
