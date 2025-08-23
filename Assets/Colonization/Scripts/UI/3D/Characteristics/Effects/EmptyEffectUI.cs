using System.Runtime.CompilerServices;
using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    sealed public class EmptyEffectUI : AEffectUI
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void GetText(Localization language, StringBuilder sb) { }
    }
}
