using System.Runtime.CompilerServices;
using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    sealed public class MainAddTwoEffectUI : AMainEffectsUI
    {
        private readonly string _advA, _advB;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MainAddTwoEffectUI(string descKey, string main, int advA, int advB, string hexColor, AEffectsUI advEffect) : base(descKey, main, hexColor, advEffect)
        {
            _advA = advA.ToString();
            _advB = advB.ToString();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MainAddTwoEffectUI(string descKey, string main, int advA, int advB, string hexColor) : this(descKey, main, advA, advB, hexColor, new EmptyEffectUI()) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MainAddTwoEffectUI(string descKey, int main, int advA, int advB, string hexColor) : this(descKey, main.ToString(), advA, advB, hexColor, new EmptyEffectUI()) { }

        public override void GetText(Localization language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetFormatText(CONST_UI_LNG_KEYS.FILE, _descKey, _main, _advA, _advB));
            _advEffect.GetText(language, sb);
        }
    }
}
