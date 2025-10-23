using System.Runtime.CompilerServices;

namespace Vurbiri.Colonization
{
	public abstract class AClearEffect : AHitEffect
    {
        protected readonly Id<ClearEffectsId> _type;
        protected readonly int _value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AClearEffect(Id<ClearEffectsId> type, int value)
        {
            _type = type;
            _value = value;
        }
    }
}
