using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    sealed public class SeparatorEffectUI : AEffectsUI
    {
        public SeparatorEffectUI(ProjectColors colors) : base(null, null, colors.TextDefaultTag)
        {
        }

        public override void GetText(Localization language, StringBuilder sb) => GetText(sb);
        
        public void GetText(StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine("─◈─");
        }
    }
}
