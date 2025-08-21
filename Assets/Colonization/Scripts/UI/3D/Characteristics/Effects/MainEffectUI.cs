using System.Runtime.CompilerServices;
using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    sealed public class MainEffectUI : AMainEffectsUI
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MainEffectUI(string descKey, string main, string hexColor, AEffectsUI advEffect) : base(descKey, main, hexColor, advEffect) { }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MainEffectUI(string descKey, string main, string hexColor) : base(descKey, main, hexColor, new EmptyEffectUI()) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MainEffectUI(string descKey, int main, string hexColor) : base(descKey, main.ToString(), hexColor, new EmptyEffectUI()) { }

        public override void GetText(Localization language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetFormatText(CONST_UI_LNG_KEYS.FILE, _descKey, _main));
            _advEffect.GetText(language, sb);
        }
    }
}
