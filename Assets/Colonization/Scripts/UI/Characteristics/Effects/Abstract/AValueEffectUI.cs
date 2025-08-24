using System.Runtime.CompilerServices;

namespace Vurbiri.Colonization.UI
{
	public abstract class AValueEffectUI : AEffectUI
    {
        protected readonly string _descKey;
        protected readonly string _hexColor;

        protected readonly AEffectUI _advEffect;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AValueEffectUI(string key, string hexColor, AEffectUI advEffect)
        {
            _descKey = key;
            _hexColor = hexColor;
            _advEffect = advEffect;
        }
    }
}
