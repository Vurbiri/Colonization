using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    public abstract class AEffectUI
	{
        public abstract void GetText(Localization language, StringBuilder sb);
    }
}
