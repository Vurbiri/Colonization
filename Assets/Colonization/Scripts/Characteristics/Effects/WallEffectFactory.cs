using System.Runtime.CompilerServices;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization
{
	public class WallEffectFactory
	{
		public static readonly EffectCode WallEffectCode = new(WALL_TYPE, 0, 0, 0);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ReactiveEffect Create(int value)
		{
			return value > 0 ? new(WallEffectCode, ActorAbilityId.Defense, TypeModifierId.Addition, value * WALL_RATE, WALL_DURATION, WALL_SKIP, true) : null;
		}
	}
}
