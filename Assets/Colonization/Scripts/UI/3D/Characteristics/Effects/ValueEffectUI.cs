using System.Runtime.CompilerServices;
using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    sealed public class ValueEffectUI : AValueEffectUI
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueEffectUI(string descKey, string value, string hexColor, AEffectUI advEffect) : base(descKey, value, hexColor, advEffect) { }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueEffectUI(string descKey, string value, string hexColor) : base(descKey, value, hexColor, new EmptyEffectUI()) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueEffectUI(string descKey, int value, string hexColor) : base(descKey, value.ToString(), hexColor, new EmptyEffectUI()) { }

        public override void GetText(Localization language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetFormatText(CONST_UI_LNG_KEYS.FILE, _descKey, _value));
            _advEffect.GetText(language, sb);
        }
    }
}
