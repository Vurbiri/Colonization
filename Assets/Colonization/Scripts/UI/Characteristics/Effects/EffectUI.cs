using System.Runtime.CompilerServices;
using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    public class EffectUI : AEffectUI
    {
        private static readonly EmptyEffectUI s_empty = new();

        protected readonly string _descKey;
        protected readonly string _hexColor;

        public static AEffectUI Empty => s_empty;

        public EffectUI(string key, string hexColor)
        {
            _descKey = key;
            _hexColor = hexColor;
        }

        public override void GetText(Localization language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetText(CONST_UI_LNG_KEYS.FILE, _descKey));
        }

        // Nested
        //********************************************
        sealed private class EmptyEffectUI : AEffectUI
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override void GetText(Localization language, StringBuilder sb) { }
        }
    }
}
