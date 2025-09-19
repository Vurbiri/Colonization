using System.Runtime.CompilerServices;
using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    sealed public class ValueEffectUI : AValueEffectUI
    {
        private readonly string _value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueEffectUI(string descKey, string value, string hexColor, AEffectUI advEffect) : base(descKey, hexColor, advEffect) => _value = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueEffectUI(string descKey, string value, string hexColor) : base(descKey, hexColor, EffectUI.Empty) => _value = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueEffectUI(string descKey, int value, string hexColor) : base(descKey, hexColor, EffectUI.Empty) => _value = value.ToString();

        public override void GetText(Localization language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetFormatText(CONST_UI.FILE, _descKey, _value));
            _advEffect.GetText(language, sb);
        }
    }
}
