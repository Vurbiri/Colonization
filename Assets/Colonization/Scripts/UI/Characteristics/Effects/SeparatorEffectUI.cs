using System.Runtime.CompilerServices;
using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    sealed public class SeparatorEffectUI : AEffectUI
    {
        private readonly string _hexColor;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SeparatorEffectUI(ProjectColors colors) => _hexColor = colors.TextDefaultTag;


        public override void GetText(Localization language, StringBuilder sb) => GetText(sb);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetText(StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine("─◈─");
        }
    }
}
