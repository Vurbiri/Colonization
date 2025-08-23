using System.Runtime.CompilerServices;
using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    sealed public class TwoValuesEffectUI : AValueEffectUI
    {
        private readonly string _adv;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TwoValuesEffectUI(string descKey, string main, int adv, string hexColor, AEffectUI advEffect) : base(descKey, main, hexColor, advEffect)
        {
            _adv = adv.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TwoValuesEffectUI(string descKey, string main, int adv, string hexColor) : this(descKey, main, adv, hexColor, new EmptyEffectUI()) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TwoValuesEffectUI(string descKey, int main, int adv, string hexColor) : this(descKey, main.ToString(), adv, hexColor, new EmptyEffectUI()) { }

        public override void GetText(Localization language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetFormatText(CONST_UI_LNG_KEYS.FILE, _descKey, _value, _adv));
            _advEffect.GetText(language, sb);
        }
    }
}
